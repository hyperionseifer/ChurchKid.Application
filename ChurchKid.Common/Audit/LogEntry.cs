using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchKid.Common.Audit
{
    public class LogEntry
    {

        public LogEntry()
        {
            Id = 0;
            DateAndTime = DateTime.Now;
            Type = LogEntryType.Info;
            Details = string.Empty;
            StackTrace = string.Empty;
        }

        public int Id { get; set; }

        public DateTime DateAndTime { get; set; }

        public string Type { get; set; }

        public string Details { get; set; }

        public string StackTrace { get; set; }

    }
}
