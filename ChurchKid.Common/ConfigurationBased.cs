using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace ChurchKid.Common
{
    public class ConfigurationBased
    {

        private static string configurationPath = string.Empty;

        public static string ConfigurationPath
        {
            get
            {
                if (string.IsNullOrEmpty(configurationPath))
                {
                    var uriAssemblyFolder = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
                    var appPath = uriAssemblyFolder.LocalPath;
                    configurationPath = string.Format("{0}\\{1}.dll", appPath, Assembly.GetExecutingAssembly().GetName().Name);
                }

                return configurationPath;
            }
            set
            {
                if (!configurationPath.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    configurationPath = value;
                    configuration = null;
                    InitializeConfiguration();
                }
            }
        }

        private static Configuration configuration;

        private static void InitializeConfiguration()
        {
            if (configuration == null)
                configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationPath);
        }

        public static AppSettingsSection AppSettingsConfiguration
        {
            get
            {
                InitializeConfiguration();
                return configuration.GetSection("appSettings") as AppSettingsSection;
            }
        }

        public static ConnectionStringsSection ConnectionStringsConfiguration
        {
            get
            {
                InitializeConfiguration();
                return configuration.GetSection("connectionStrings") as ConnectionStringsSection;
            }
        }

    }

}
