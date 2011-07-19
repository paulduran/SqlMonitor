using System;
using System.Collections.Generic;
using SqlMonitor.Core.Domain;

namespace SqlMonitor.Core.DataInterfaces
{
    public interface IQueryDao
    {
        Query GetByName(string name);
        void Add(Query query);
        IEnumerable<Query> GetQueries();
        void UpdateRunDates(string name, DateTime lastRun, DateTime? nextRun);
        Query GetById(int id);
        void UpdateQuery(Query query);
    }
}