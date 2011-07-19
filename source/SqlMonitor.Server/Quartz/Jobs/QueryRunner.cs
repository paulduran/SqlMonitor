using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using NServiceBus;
using Quartz;
using Spring.Data.Common;
using Spring.Data.Core;
using Spring.Data.Support;
using SqlMonitor.Messages.Commands;
using SqlMonitor.Messages.Events;

namespace SqlMonitor.Server.Quartz.Jobs
{
    public class QueryRunner : IStatefulJob
    {
        private readonly IBus bus;

        public QueryRunner(IBus bus)
        {
            this.bus = bus;
        }

        public void Execute(JobExecutionContext context)
        {
            JobDataMap map = context.MergedJobDataMap;

            ScheduleQuery details = (ScheduleQuery) map["Details"];

            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[details.DatabaseName];
            if (connectionStringSettings != null)
            {
                var nextRunDate = GetNextRunDate(context.Trigger);
                AdoTemplate template = CreateTemplate(connectionStringSettings);
                DateTime startTime = DateTime.Now;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    int numResults = RunQuery(template, details.QueryText);
                    stopwatch.Stop();
                    PublishSuccessEvent(details, numResults, startTime, nextRunDate, stopwatch.Elapsed);
                } catch (Exception ex)
                {
                    PublishFailEvent(details, startTime, nextRunDate, ex);
                }
            }
        }

        private static DateTime? GetNextRunDate(Trigger trigger)
        {
            var nextFire = trigger.GetNextFireTimeUtc();
            return nextFire == null ? (DateTime?) null : nextFire.Value.ToLocalTime();
        }

        private static AdoTemplate CreateTemplate(ConnectionStringSettings connectionStringSettings)
        {
            var dbProvider = DbProviderFactory.GetDbProvider(connectionStringSettings.ProviderName);
            dbProvider.ConnectionString = connectionStringSettings.ConnectionString;
            return new AdoTemplate(dbProvider)
                   {DataReaderWrapperType = typeof (NullMappingDataReader), CommandTimeout = 120};
        }

        private void PublishSuccessEvent(ScheduleQuery details, int numResults, DateTime startDate, DateTime? nextRunDate, TimeSpan elapsed)
        {
            var result = bus.CreateInstance<IQueryResult>(x =>
                                                          {
                                                              x.Success = true;
                                                              x.Duration = elapsed;
                                                              x.StartDate = startDate;
                                                              x.Name = details.Name;
                                                              x.NumberOfResults = numResults;
                                                              x.NextRun = nextRunDate;
                                                              x.ThresholdMilliseconds = details.ThresholdMilliseconds;
                                                              x.AlertIfAboveThreshold = details.AlertIfAboveThreshold;
                                                              x.AlertEmailTo = details.AlertEmailTo;
                                                          });
            bus.Publish(result);
        }

        private void PublishFailEvent(ScheduleQuery details, DateTime startDate, DateTime? nextRunDate, Exception exception)
        {
            var result = bus.CreateInstance<IQueryResult>(x =>
                                                          {
                                                              x.StartDate = startDate;
                                                              x.Name = details.Name;
                                                              x.NextRun = nextRunDate;
                                                              x.Success = false;
                                                              x.ErrorMessage = exception.Message;
                                                              x.AlertIfAboveThreshold = details.AlertIfAboveThreshold;
                                                              x.AlertEmailTo = details.AlertEmailTo;
                                                          });
            bus.Publish(result);
        }

        private static int RunQuery(AdoTemplate template, string query)
        {
            int rowCount = 0;
            template.QueryWithRowCallbackDelegate(CommandType.Text, query, r=>rowCount++);
            return rowCount;
        }
    }
}
