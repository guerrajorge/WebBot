using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LinqToWikipedia
{
    /// <summary>
    /// Write exception information to the event log
    /// </summary>
    internal static class Logger
    {
        internal static void WriteToEventLog(string message)
        {
            WriteToEventLog(message, null, EventLogEntryType.Warning);
        }

        internal static void WriteToEventLog(string message, Exception wikiException)
        {
            WriteToEventLog(message, wikiException, EventLogEntryType.Warning);
        }

        internal static void WriteToEventLog(string message, Exception wikiException, EventLogEntryType logtype)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(message);

            if (wikiException != null)
            {
                sb.Append("\n\n");
                sb.Append("Source: " + wikiException.Source + "\n\n");
                sb.Append("Stacktrace: " + wikiException.StackTrace + "\n\n");

                if (wikiException.InnerException != null)
                {
                    sb.Append("Inner Exception: \n\n");
                    sb.Append("Source: " + wikiException.InnerException.Source + "\n\n");
                    sb.Append("Stacktrace: " + wikiException.InnerException.StackTrace + "\n\n");
                }
            }

            try
            {
                EventLog.WriteEntry("application", sb.ToString(), logtype);
            }
            catch 
            {
                // do nothing in case application identity does not have access to write to Event Log
            }
        }
    }
}
