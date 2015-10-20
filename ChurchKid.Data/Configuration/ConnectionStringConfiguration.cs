using ChurchKid.Common;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace ChurchKid.Data.Configuration
{
    public abstract class ConnectionStringConfiguration : ConfigurationBased
    {

        private static void SetConfigurationPath()
        {
            var uriAssemblyFolder = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
            var appPath = uriAssemblyFolder.LocalPath;
            var properPath = string.Format("{0}\\{1}.dll", appPath, Assembly.GetExecutingAssembly().GetName().Name);
            ConfigurationPath = properPath;
        }

        public static ConnectionStringSettings DefaultConnectionString
        {
            get
            {
                SetConfigurationPath();
                return ConnectionStringsConfiguration.ConnectionStrings["DefaultConnection"];
            }
        }

        public static ConnectionStringSettings FTTConnectionString
        {
            get
            {
                SetConfigurationPath();
                return ConnectionStringsConfiguration.ConnectionStrings["FTTConnection"];
            }
        }

    }
}
