using System;
using AutoMapper;
using Bootstrap;
using NServiceBus;
using SqlMonitor.Core.DataInterfaces;
using SqlMonitor.Core.Domain;
using SqlMonitor.Messages.Commands;

namespace SqlMonitor.Server.BootStrappers
{
    public class RescheduleJobs : IWantToRunAtStartup
    {
        private readonly IBus bus;
        private readonly IQueryDao queryDao;

        public RescheduleJobs(IBus bus, IQueryDao queryDao)
        {
            this.bus = bus;
            this.queryDao = queryDao;
        }

        public void Run()
        {
            foreach (var query in queryDao.GetQueries())
            {
                var scheduleQuery = Mapper.Map<Query, ScheduleQuery>(query);
                bus.Publish(scheduleQuery);
            }
        }

        public void Stop()
        {
            
        }
    }
}
