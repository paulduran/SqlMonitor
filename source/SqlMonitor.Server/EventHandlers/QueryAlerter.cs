using System;
using System.Net.Mail;
using NServiceBus;
using SqlMonitor.Core;
using SqlMonitor.Messages.Events;

namespace SqlMonitor.Server.EventHandlers
{
    public class QueryAlerter : IHandleMessages<IQueryResult>
    {
        private readonly SmtpClient smtpClient;

        public QueryAlerter(SmtpClient smtpClient)
        {
            this.smtpClient = smtpClient;
            string fromName;
            string fromEmail;
            string fromSubject;
        }

        public void Handle(IQueryResult message)
        {
            if( ! message.Success )
                return;
            if( message.Duration.TotalMilliseconds > message.ThresholdMilliseconds && message.AlertIfAboveThreshold )
            {
                string subject = string.Format("[Harrier] Query Alert - {0} - above threshold", message.Name);
                var threshold = new TimeSpan(0,0,0,0, message.ThresholdMilliseconds);                
                string body = string.Format("Query Name: {0}\nQuery Run At: {1}\nThreshold: {2}\nTime Taken: {3}\n\nRegards,\nHarrier Alerts", message.Name, message.StartDate,
                                            DateHelpers.FormatTimespan(threshold), DateHelpers.FormatTimespan(message.Duration));
                const string from = "HarrierAlerts@onepath.com.au";
                smtpClient.Send(from, message.AlertEmailTo, subject, body);
            }            
        }
    }
}