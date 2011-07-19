using System;
using System.Configuration;
using System.Net.Mail;
using NServiceBus;
using SqlMonitor.Core;
using SqlMonitor.Messages.Events;

namespace SqlMonitor.Server.EventHandlers
{
    public class QueryAlerter : IHandleMessages<IQueryResult>
    {
        private readonly SmtpClient smtpClient;
        private readonly string fromName;
        private readonly string fromEmail;
        private readonly string subject;

        public QueryAlerter(SmtpClient smtpClient)
        {
            this.smtpClient = smtpClient;
            fromName  = ConfigurationManager.AppSettings["AlertFromName"];
            fromEmail = ConfigurationManager.AppSettings["AlertFromEmail"];
            subject = ConfigurationManager.AppSettings["AlertSubject"];
        }

        public void Handle(IQueryResult message)
        {
            if( ! message.Success )
                return;
            if( message.Duration.TotalMilliseconds > message.ThresholdMilliseconds && message.AlertIfAboveThreshold )
            {
                var formattedSubject = string.Format(subject, message.Name);
                var threshold = new TimeSpan(0,0,0,0, message.ThresholdMilliseconds);                
                string body = string.Format("Query Name: {0}\nQuery Run At: {1}\nThreshold: {2}\nTime Taken: {3}\n\nRegards,\n{4}", message.Name, message.StartDate,
                                            DateHelpers.FormatTimespan(threshold), DateHelpers.FormatTimespan(message.Duration), fromName);
                smtpClient.Send(fromEmail, message.AlertEmailTo, formattedSubject, body);
            }            
        }
    }
}