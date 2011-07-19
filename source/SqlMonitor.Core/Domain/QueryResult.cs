using System;

namespace SqlMonitor.Core.Domain
{  
    public class QueryResult
    {
        public int QueryResultId { get; set; }
        public DateTime StartDate { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public TimeSpan Duration { get; set; }
        public int NumberOfResults { get; set; }
        public Query Query { get; set; }
    }
}
