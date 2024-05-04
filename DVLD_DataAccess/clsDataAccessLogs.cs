using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsDataAccessLogs
    {

        // use event log to log the exception.
        public static void CreateExceptionLog(string Message)
        {

            string SourceName = "DVLD";

            // Create the event source if it does not exist
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, "Application");

            }
            EventLog.WriteEntry(SourceName, Message, EventLogEntryType.Error);
        }



    }
}
