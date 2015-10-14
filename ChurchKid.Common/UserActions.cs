using System;
using System.Configuration;

namespace ChurchKid.Common
{
    public abstract class UserActions
    {

        public static string Add
        {
            get
            {
                return (ConfigurationManager.AppSettings["UserActions.Add"] ?? "Add");
            }
        }

        public static string Edit
        {
            get
            {
                return (ConfigurationManager.AppSettings["UserActions.Edit"] ?? "Edit");
            }
        }

        public static string Delete
        {
            get
            {
                return (ConfigurationManager.AppSettings["UserActions.Delete"] ?? "Delete");
            }
        }

        public static string Print
        {
            get
            {
                return (ConfigurationManager.AppSettings["UserActions.Print"] ?? "Print");
            }
        }

        public static string Approve
        {
            get
            {
                return (ConfigurationManager.AppSettings["UserActions.Approve"] ?? "Approve");
            }
        }

        public static string Decline
        {
            get
            {
                return (ConfigurationManager.AppSettings["UserActions.Decline"] ?? "Decline");
            }
        }

        public static string Draft
        {
            get
            {
                return (ConfigurationManager.AppSettings["UserActions.Draft"] ?? "Draft");
            }
        }

        public static string Backup
        {
            get
            {
                return ConfigurationManager.AppSettings["UserActions.Backup"];
            }
        }

    }

}
