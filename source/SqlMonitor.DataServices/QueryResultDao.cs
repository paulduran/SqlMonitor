using System.Linq;
using SqlMonitor.Core.DataInterfaces;
using SqlMonitor.Core.Domain;

namespace SqlMonitor.DataServices
{
    public class QueryResultDao : IQueryResultDao
    {
        public void Add(QueryResult queryResult, string name)
        {
            using (var db = new QueryContext())
            {
                queryResult.Query = db.Queries.FirstOrDefault(x => x.Name == name);
                db.QueryResults.Add(queryResult);
                db.SaveChanges();
            }
        }
    }
}