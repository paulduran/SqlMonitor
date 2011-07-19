using AutoMapper;
using log4net;
using NServiceBus;
using SqlMonitor.Core.DataInterfaces;
using SqlMonitor.Core.Domain;
using SqlMonitor.Messages.Events;

namespace SqlMonitor.Server.EventHandlers
{
    public class QueryResultLogger : IHandleMessages<IQueryResult>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (QueryResultLogger));
        private readonly IQueryDao queryDao;
        private readonly IQueryResultDao queryResultDao;

        public QueryResultLogger(IQueryDao queryDao, IQueryResultDao queryResultDao)
        {
            this.queryDao = queryDao;
            this.queryResultDao = queryResultDao;
        }

        public void Handle(IQueryResult message)
        {
            log.Info(string.Format("Query Result Received. Name: {0}. Number of Rows: {1}. Duration: {2}", message.Name, message.NumberOfResults, message.Duration));
            QueryResult result = Mapper.Map<IQueryResult, QueryResult>(message);            
//            result.Query = queryDao.GetByName(message.Name);
            queryResultDao.Add(result, message.Name);
        }
    }
}
