using System;
using System.Web.Mvc;

namespace SqlMonitor.Web.Helpers
{
    public static class DateHelpers
    {
        public static string FormatTimespan(this HtmlHelper helper, TimeSpan timespan)
        {
            return Core.DateHelpers.FormatTimespan(timespan);
        }
    }
}