using ChurchKid.Common.Audit;
using ChurchKid.Common.Resources;
using ChurchKid.Data.Entities.Audit;
using System;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;

namespace ChurchKid.Data.Seed
{
    public class ApplicationModules : ISeeder
    {

        private DatabaseConnection connection;

        public ApplicationModules(DatabaseConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.ApplicationModules;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            try
            {
                if (!connection.ApplicationModules.Any())
                {
                    var data = XElement.Parse(SeedData);
                    var modules = from appModule in data.Elements("applicationModule")
                                  select new
                                  {
                                      GroupName = (string)appModule.Element("group"),
                                      Name = (string)appModule.Element("name")
                                  };

                    if (modules.Any())
                    {
                        var persistedModuleGroups = connection.ApplicationModuleGroups.ToList();
                        if (!persistedModuleGroups.Any())
                            throw new Exception(ApplicationStrings.errNoModuleGroupsAvailable);

                        var administratorId = 0;
                        var administrator = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCultureIgnoreCase));
                        if (administrator != null)
                            administratorId = administrator.UserId;

                        using (var transaction = new TransactionScope())
                        {
                            foreach (var module in modules)
                            {
                                var moduleGroup = persistedModuleGroups.FirstOrDefault(g => module.GroupName.Equals(g.Name, StringComparison.InvariantCultureIgnoreCase));
                                connection.ApplicationModules.Add(new ApplicationModule()
                                {
                                    ApplicationModuleGroupId = moduleGroup.ApplicationModuleGroupId,
                                    Name = module.Name,
                                    CreatedById = administratorId
                                });
                            }

                            connection.SaveChanges();
                            transaction.Complete();
                            Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataApplicationModules));
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
