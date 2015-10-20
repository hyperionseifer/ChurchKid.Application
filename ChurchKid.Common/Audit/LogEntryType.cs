using System;
using System.IO;
using System.Reflection;

namespace ChurchKid.Common.Audit
{
    public abstract class LogEntryType : ConfigurationBased
    {

        private static void SetConfigurationPath()
        {
            var uriAssemblyFolder = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
            var appPath = uriAssemblyFolder.LocalPath;
            var properPath = string.Format("{0}\\{1}.dll", appPath, Assembly.GetExecutingAssembly().GetName().Name);
            ConfigurationPath = properPath;
        }

        public static string Error
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["LogEntryTypes.Error"].Value ?? "Error");
            }
        }

        public static string Info
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["LogEntryTypes.Info"].Value ?? "Info");
            }
        }

        public static string Warning
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["LogEntryTypes.Warning"].Value ?? "Warning");
            }
        }
    }
}
