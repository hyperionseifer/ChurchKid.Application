using ChurchKid.Common.Audit;
using ChurchKid.Common.Resources;
using ChurchKid.Data.Entities.Audit;
using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;

namespace ChurchKid.Data.Seed
{
    public class ApplicationModuleGroups : ISeeder
    {

        private DatabaseConnection connection;

        public ApplicationModuleGroups(DatabaseConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.ApplicationModuleGroups;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            try
            {
                if (!connection.ApplicationModuleGroups.Any())
                {

                    var data = XElement.Parse(SeedData);
                    var moduleGroups = from appModuleGroup in data.Elements("applicationModuleGroup")
                                       select new
                                       {
                                           Name = (string)appModuleGroup.Element("name")
                                       };

                    if (moduleGroups.Any())
                    {
                        var administratorId = 0;
                        var administrator = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCultureIgnoreCase));
                        if (administrator != null)
                            administratorId = administrator.UserId;

                        var groups = from moduleGroup in moduleGroups
                                     select new ApplicationModuleGroup()
                                     {
                                         Name = moduleGroup.Name,
                                         CreatedById = administratorId
                                     };

                        using (var transaction = new TransactionScope())
                        {
                            connection.ApplicationModuleGroups.AddRange(groups);
                            connection.SaveChanges();
                            transaction.Complete();
                            Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataApplicationModuleGroups));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
        }

    }
}
