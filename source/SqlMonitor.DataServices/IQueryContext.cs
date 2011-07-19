using System.Data.Entity;
using SqlMonitor.Core.Domain;

namespace SqlMonitor.DataServices
{
    public interface IQueryContext
    {
        DbSet<Query> Queries { get; set; }
        DbSet<QueryResult> QueryResults { get; set; }
        int SaveChanges();
    }
}