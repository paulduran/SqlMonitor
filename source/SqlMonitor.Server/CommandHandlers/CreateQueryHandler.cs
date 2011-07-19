using AutoMapper;
using NServiceBus;
using SqlMonitor.Server.Quartz.Jobs;
using Quartz;
using SqlMonitor.Core.DataInterfaces;
using SqlMonitor.Core.Domain;
using SqlMonitor.Messages.Commands;

namespace SqlMonitor.Server.CommandHandlers
{
    public class CreateQueryHandler : IHandleMessages<CreateQuery>
    {
        private readonly IBus bus;
        private readonly IQueryDao queryDao;

        public CreateQueryHandler(IBus bus, IQueryDao queryDao)
        {
            this.bus = bus;
            this.queryDao = queryDao;
        }

        public void Handle(CreateQuery message)
        {
            var query = Mapper.Map<CreateQuery, Query>(message);
            queryDao.Add(query);

            var schedule = Mapper.Map<Query, ScheduleQuery>(query);            
            bus.Send(schedule);
        }
    }
}
