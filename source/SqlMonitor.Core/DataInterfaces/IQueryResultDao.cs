using SqlMonitor.Core.Domain;

namespace SqlMonitor.Core.DataInterfaces
{
    public interface IQueryResultDao
    {
        void Add(QueryResult queryResult, string name);
    }
}