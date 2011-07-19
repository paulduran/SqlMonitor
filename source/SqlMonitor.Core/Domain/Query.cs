using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SqlMonitor.Core.Domain
{
    public class Query
    {
        public int QueryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Database Name")]
        public string DatabaseName { get; set; }
        [Required]
        [Display(Name = "Query Text")]
        public string QueryText { get; set; }
        [Required]
        [Display(Name = "Cron Expression")]
        public string CronExpression { get; set; }

        [Display(Name = "Last Run")]
        public DateTime? LastRun { get; set; }

        [Display(Name = "Next Run")]
        public DateTime? NextRun { get; set; }

        [Display(Name = "Threshold (in ms)")]
        public int ThresholdMilliseconds { get; set; }

        [Display(Name = "Alert if above threshold?")]
        public bool AlertIfAboveThreshold { get; set; }

        [Display(Name = "Email To")]
        [DataType(DataType.EmailAddress)]
        public string AlertEmailTo { get; set; }
        
        public virtual ICollection<QueryResult> Results { get; set; }
    }
}