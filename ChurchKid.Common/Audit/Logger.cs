using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;

namespace ChurchKid.Common.Audit
{
    public abstract class Logger
    {

        
        private static void CreateLogFile(string path)
        {
            using (var logTable = new DataTable())
            {
                logTable.TableName = "logs";

                DataColumn keyColumn = logTable.Columns.Add("Id", typeof(int));
                keyColumn.AutoIncrement = true;
                keyColumn.AutoIncrementSeed = 1;
                logTable.Constraints.Add("logs_Id", keyColumn, true);

                logTable.Columns.Add("DateAndTime", typeof(DateTime));
                logTable.Columns.Add("Type", typeof(string));
                logTable.Columns.Add("Details", typeof(string));
                logTable.Columns.Add("StackTrace", typeof(string));

                try 
                {
                    logTable.WriteXml(path, XmlWriteMode.WriteSchema);
                }
                catch
                { }
            }            
        }

        private static DataTable GetLogTable()
        {
            var logTable = new DataTable();
            var logFile = LogFile;

            if (File.Exists(logFile))
            {
                try
                {
                    logTable.ReadXml(logFile);
                }
                catch
                {
                    logTable.Dispose();
                    logTable = null;
                }
            }

            return logTable;
        }

        public static string LogFile
        {
            get
            {
                var logFile = ConfigurationManager.AppSettings["LogFile"]; ;
                if (string.IsNullOrEmpty(logFile))
                    logFile = "ChurchKid.Logs.xml";

                var directory = Path.GetDirectoryName(logFile);
                if (string.IsNullOrEmpty(directory))
                    directory = Environment.CurrentDirectory;

                var directoryExists = true;

                if (!Directory.Exists(directory))
                {
                    try
                    {
                        Directory.CreateDirectory(directory);
                    }
                    catch
                    { 
                        directoryExists = false; 
                    }
                }

                if (directoryExists)
                {
                    var fileName = string.Format("{0}\\{1}", directory, Path.GetFileName(logFile));
                    if (!File.Exists(fileName))
                        CreateLogFile(fileName);
                }

                return logFile;
            }
        }

        public static void Clear()
        {
            var logFile = LogFile;
            var logTable = GetLogTable();
            if (logTable != null)
            {
                using (logTable)
                {
                    logTable.Rows.Clear();
                    logTable.AcceptChanges();
                    logTable.WriteXml(logFile, XmlWriteMode.WriteSchema);
                }
            }
            else
            {
                if (File.Exists(logFile))
                {
                    try
                    {
                        File.Delete(logFile);
                    }
                    catch
                    { }
                }
            }
        }

        public static LogEntry GetLog(int id)
        {
            var logEntries = GetLogs(string.Format("[Id] = {0}", id));
            if (logEntries.Any())
                return logEntries.FirstOrDefault();

            return null;
        }

        public static IEnumerable<LogEntry> GetErrorEntries()
        {
            return GetLogs(string.Format("[Type] = '{0}'", LogEntryType.Error));
        }

        public static IEnumerable<LogEntry> GetInfoEntries()
        {
            return GetLogs(string.Format("[Type] = '{0}'", LogEntryType.Info));
        }

        public static IEnumerable<LogEntry> GetWarningEntries()
        {
            return GetLogs(string.Format("[Type] = '{0}'", LogEntryType.Warning));
        }

        public static IEnumerable<LogEntry> GetLogs()
        {
            return GetLogs(string.Empty);
        }

        public static IEnumerable<LogEntry> GetLogs(string filter)
        {
            var logEntries = new List<LogEntry>();

            DataTable logTable = GetLogTable();
            if (logTable != null)
            {
                using (logTable)
                {
                   logTable.DefaultView.RowFilter = filter;
                   using (var filteredTable = logTable.DefaultView.ToTable())
                   {
                       logEntries = (from logEntry in filteredTable.AsEnumerable()
                                     select new LogEntry()
                                     {
                                         Id = logEntry.Field<int>("Id"),
                                         DateAndTime = logEntry.Field<DateTime>("DateAndTime"),
                                         Type = logEntry.Field<string>("Type"),
                                         Details = logEntry.Field<string>("Details"),
                                         StackTrace = logEntry.Field<string>("StackTrace")
                                     }).ToList();
                   }
                }
            }

            return logEntries;
        }

        public static LogEntry Write(Exception ex)
        {
            return Write(new LogEntry() 
            {
                Type = LogEntryType.Error,
                Details = ex.Message,
                StackTrace = ex.StackTrace
            });
        }

        public static LogEntry Write(string details)
        {
            return Write(LogEntryType.Info, details);
        }

        public static LogEntry Write(string type, string details)
        {
            return Write(type, details, string.Empty);
        }

        public static LogEntry Write(string type, string details, string stackTrace)
        {
            return Write(new LogEntry()
            {
                Type = type, 
                Details = details,
                StackTrace = stackTrace
            });
        }

        public static LogEntry Write(LogEntry logEntry)
        {
            if (logEntry == null)
                return null;

            var logTable = GetLogTable();
            if (logTable == null)
                return null;

            var newLogEntry = new LogEntry()
            {
                DateAndTime = logEntry.DateAndTime,
                Type = logEntry.Type,
                Details = logEntry.Details,
                StackTrace = logEntry.StackTrace
            };

            using (logTable)
            {
                try
                {
                    DataRow newRow = logTable.Rows.Add(null, logEntry.DateAndTime, logEntry.Type, logEntry.Details, logEntry.StackTrace);
                    logTable.AcceptChanges();
                    logTable.WriteXml(LogFile, XmlWriteMode.WriteSchema);
                    newLogEntry.Id = Convert.ToInt32(newRow["Id"]);
                }
                catch
                {
                    return null;
                }
            }

            return newLogEntry;
        }

    }
}
