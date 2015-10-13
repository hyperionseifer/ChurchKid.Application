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
                return ConfigurationManager.AppSettings["UserActions.Add"];
            }
        }

        public static string Edit
        {
            get
            {
                return ConfigurationManager.AppSettings["UserActions.Edit"];
            }
        }

        public static string Delete
        {
            get
            {
                return ConfigurationManager.AppSettings["UserActions.Delete"];
            }
        }

        public static string Print
        {
            get
            {
                return ConfigurationManager.AppSettings["UserActions.Print"];
            }
        }

        public static string Approve
        {
            get
            {
                return ConfigurationManager.AppSettings["UserActions.Approve"];
            }
        }

        public static string Decline
        {
            get
            {
                return ConfigurationManager.AppSettings["UserActions.Decline"];
            }
        }

        public static string Draft
        {
            get
            {
                return ConfigurationManager.AppSettings["UserActions.Draft"];
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
