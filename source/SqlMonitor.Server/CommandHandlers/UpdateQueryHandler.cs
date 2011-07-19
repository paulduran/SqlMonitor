using AutoMapper;
using NServiceBus;
using SqlMonitor.Core.DataInterfaces;
using SqlMonitor.Core.Domain;
using SqlMonitor.Messages.Commands;

namespace SqlMonitor.Server.CommandHandlers
{
    public class UpdateQueryHandler: IHandleMessages<UpdateQuery>
    {
        private readonly IBus bus;
        private readonly IQueryDao queryDao;

        public UpdateQueryHandler(IBus bus, IQueryDao queryDao)
        {
            this.bus = bus;
            this.queryDao = queryDao;
        }

        public void Handle(UpdateQuery message)
        {
            var query = Mapper.Map<UpdateQuery, Query>(message);
            var origQuery = queryDao.GetById(query.QueryId);            
            string origName = origQuery.Name;
            queryDao.UpdateQuery(query);

            var schedule = Mapper.Map<Query, ScheduleQuery>(query);
            if( origName != query.Name )
                schedule.OriginalName = origName;
            bus.Send(schedule);
        }
    }
}
