using System;
using NServiceBus;

namespace SqlMonitor.Messages.Events
{
    public interface IQueryResult : IMessage
    {
        string Name { get; set; }
        DateTime StartDate { get; set; }
        bool Success { get; set; }
        string ErrorMessage { get; set; }
        TimeSpan Duration { get; set; }
        int NumberOfResults { get; set; }
        DateTime? NextRun { get; set; }
        int ThresholdMilliseconds { get; set; }
        bool AlertIfAboveThreshold { get; set; }
        string AlertEmailTo { get; set; }
    }
}
