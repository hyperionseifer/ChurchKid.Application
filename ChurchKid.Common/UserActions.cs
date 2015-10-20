using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ChurchKid.Common
{
    public abstract class UserActions : ConfigurationBased
    {

        private static void SetConfigurationPath()
        {
            var uriAssemblyFolder = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
            var appPath = uriAssemblyFolder.LocalPath;
            var properPath = string.Format("{0}\\{1}.dll", appPath, Assembly.GetExecutingAssembly().GetName().Name);
            ConfigurationPath = properPath;
        }

        public static string Add
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["UserActions.Add"].Value ?? "Add");
            }
        }

        public static string Edit
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["UserActions.Edit"].Value ?? "Edit");
            }
        }

        public static string Delete
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["UserActions.Delete"].Value ?? "Delete");
            }
        }

        public static string Print
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["UserActions.Print"].Value ?? "Print");
            }
        }

        public static string Approve
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["UserActions.Approve"].Value ?? "Approve");
            }
        }

        public static string Decline
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["UserActions.Decline"].Value ?? "Decline");
            }
        }

        public static string Draft
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["UserActions.Draft"].Value ?? "Draft");
            }
        }

        public static string Backup
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["UserActions.Backup"].Value ?? "Backup");
            }
        }

        public static string Login
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["UserActions.Login"].Value ?? "Login");
            }
        }

        public static string Logout
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["UserActions.Logout"].Value ?? "Logout");
            }
        }

        public static string Unknown
        {
            get
            {
                SetConfigurationPath();
                return (AppSettingsConfiguration.Settings["UserActions.Unknown"].Value ?? "Unknown");
            }
        }

        public static bool IsValidAction(string action)
        {
            var actions = new string[] {
                Add, Edit, Delete, 
                Print, Approve, Decline, Draft,
                Backup, Login, Logout, Unknown
            };

            return actions.Any(a => a.Equals(action, StringComparison.InvariantCultureIgnoreCase));
        }

    }

}
