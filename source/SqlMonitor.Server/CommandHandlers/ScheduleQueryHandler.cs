using NServiceBus;
using Quartz;
using SqlMonitor.Messages.Commands;
using SqlMonitor.Server.Quartz.Jobs;

namespace SqlMonitor.Server.CommandHandlers
{
    class ScheduleQueryHandler : IHandleMessages<ScheduleQuery>
    {
        private readonly IScheduler scheduler;

        public ScheduleQueryHandler(IScheduler scheduler)
        {
            this.scheduler = scheduler;
        }

        public void Handle(ScheduleQuery message)
        {
            if( message.OriginalName != null )
            {
                scheduler.DeleteJob(message.OriginalName, null);
            }
            if( scheduler.GetJobDetail(message.Name, null) != null )
            {
                scheduler.DeleteJob(message.Name, null);
            }

            var detail = new JobDetail(message.Name, typeof (QueryRunner));
            detail.JobDataMap["Details"] = message;
            Trigger trigger = new CronTrigger("cron-" + message.Name, null, message.CronExpression);
            scheduler.ScheduleJob(detail, trigger);
        }
    }
}
