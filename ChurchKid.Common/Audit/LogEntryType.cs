using System.Configuration;

namespace ChurchKid.Common.Audit
{
    public abstract class LogEntryType
    {
        public static string Error
        {
            get
            {
                return (ConfigurationManager.AppSettings["LogEntryTypes.Error"] ?? "Error");
            }
        }

        public static string Info
        {
            get
            {
                return (ConfigurationManager.AppSettings["LogEntryTypes.Info"] ?? "Info");
            }
        }

        public static string Warning
        {
            get
            {
                return (ConfigurationManager.AppSettings["LogEntryTypes.Warning"] ?? "Warning");
            }
        }
    }
}
