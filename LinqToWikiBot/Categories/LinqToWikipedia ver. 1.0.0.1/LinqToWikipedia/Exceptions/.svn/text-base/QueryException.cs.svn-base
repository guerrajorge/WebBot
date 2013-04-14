using System;
using System.Diagnostics;

namespace LinqToWikipedia
{
    /// <summary>
    /// Custom exceptions
    /// </summary>
    class LinqToWikipediaException : System.Exception
    {
        public LinqToWikipediaException(string message)
            : base(message)
        {
            Logger.WriteToEventLog(message);
        }

        public LinqToWikipediaException(string message, Exception wikiException)
            : base(message, wikiException)
        {
            Logger.WriteToEventLog(message, wikiException);
        }

        public LinqToWikipediaException(string message, Exception wikiException, EventLogEntryType logtype)
            : base(message, wikiException)
        {
            Logger.WriteToEventLog(message, wikiException, logtype);
        }
    }
}
