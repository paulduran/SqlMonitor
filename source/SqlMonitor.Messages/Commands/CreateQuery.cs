using System;
using NServiceBus;

namespace SqlMonitor.Messages.Commands
{
    [Serializable]
    public class CreateQuery : IMessage
    {
        public string Name { get; set; }
        public string DatabaseName { get; set; }
        public string QueryText { get; set; }
        public string CronExpression { get; set; }
        public int ThresholdMilliseconds { get; set; }
        public bool AlertIfAboveThreshold { get; set; }
        public string AlertEmailTo { get; set; }
    }
}
