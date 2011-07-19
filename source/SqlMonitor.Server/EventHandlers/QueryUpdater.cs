using NServiceBus;
using SqlMonitor.Core.Domain;
using SqlMonitor.Core.DataInterfaces;
using SqlMonitor.Messages.Events;

namespace SqlMonitor.Server.EventHandlers
{
    public class QueryUpdater : IHandleMessages<IQueryResult>
    {
        private readonly IQueryDao queryDao;

        public QueryUpdater(IQueryDao queryDao)
        {
            this.queryDao = queryDao;            
        }

        public void Handle(IQueryResult message)
        {
            queryDao.UpdateRunDates(message.Name, message.StartDate, message.NextRun);
        }
    }
}
