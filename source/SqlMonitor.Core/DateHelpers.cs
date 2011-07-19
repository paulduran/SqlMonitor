using System;

namespace SqlMonitor.Core
{
    public static class DateHelpers
    {
        public static string FormatTimespan(TimeSpan timespan)
        {
            if (timespan.TotalHours >= 1)
            {
                return string.Format("{0} hrs {1} min {2}.{3} sec", timespan.TotalHours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
            }
            if (timespan.TotalMinutes >= 1)
            {
                return string.Format("{0} min {1}.{2} sec", timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
            }
            if (timespan.TotalSeconds >= 1)
            {
                return string.Format("{0}.{1} sec", timespan.Seconds, timespan.Milliseconds);
            }
            return string.Format("{0} ms", timespan.TotalMilliseconds);
        }
    }
}