using System;
using System.Collections.Generic;
using System.Linq;
using SqlMonitor.Core.DataInterfaces;
using SqlMonitor.Core.Domain;

namespace SqlMonitor.DataServices
{
    public class QueryDao : IQueryDao
    {
        public void Add(Query query)
        {
            using(var db = new QueryContext())
            {
                db.Queries.Add(query);
                db.SaveChanges();
            }
        }

        public Query GetByName(string name)
        {
            using (var db = new QueryContext())
            {
                return (from query in db.Queries
                       where query.Name == name
                       select query).FirstOrDefault();
            }
        }

        public IEnumerable<Query> GetQueries()
        {
            using (var db = new QueryContext())
            {
                return (from query in db.Queries
                       select query).ToList();
            }
        }

        public void UpdateRunDates(string name, DateTime lastRun, DateTime? nextRun)
        {
            using (var db = new QueryContext())
            {
                var query = db.Queries.FirstOrDefault(x => x.Name == name);
                if (query == null)
                    return;

                if( query.LastRun == null || lastRun > query.LastRun )
                query.LastRun = lastRun;

                query.NextRun = nextRun;
                db.SaveChanges();
            }
        }

        public Query GetById(int id)
        {
            using(var db = new QueryContext())
            {
                var q =db.Queries.Find(id); 
                foreach(var r in q.Results )
                {
                    
                }
                return q;
            }
        }

        public void UpdateQuery(Query query)
        {
            using (var db = new QueryContext())
            {
                var q = db.Queries.Find(query.QueryId);
                q.Name = query.Name;
                q.DatabaseName = query.DatabaseName;
                q.AlertEmailTo = query.AlertEmailTo;
                q.AlertIfAboveThreshold = query.AlertIfAboveThreshold;
                q.CronExpression = query.CronExpression;
                q.QueryText = query.QueryText;
                q.ThresholdMilliseconds = query.ThresholdMilliseconds;

                db.SaveChanges();
            }
        }
    }
}