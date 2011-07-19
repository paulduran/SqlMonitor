using System;
using NServiceBus;

namespace SqlMonitor.Messages.Commands
{
    [Serializable]
    public class UpdateQuery : IMessage
    {
        public int QueryId { get; set; }
        public string Name { get; set; }
        public string DatabaseName { get; set; }
        public string QueryText { get; set; }
        public string CronExpression { get; set; }
        public int ThresholdMilliseconds { get; set; }
        public bool AlertIfAboveThreshold { get; set; }
        public string AlertEmailTo { get; set; }
    }
}