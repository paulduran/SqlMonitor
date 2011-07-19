using System.Data.Entity;
using SqlMonitor.Core.Domain;

namespace SqlMonitor.DataServices
{
    public class QueryContext : DbContext, IQueryContext
    {
        public QueryContext() : base("SqlMonitor")
        {}

        public DbSet<Query> Queries { get; set; }
        public DbSet<QueryResult> QueryResults { get; set; }
    }
}