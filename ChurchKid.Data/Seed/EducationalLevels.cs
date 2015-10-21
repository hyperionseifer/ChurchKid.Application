using ChurchKid.Common;
using ChurchKid.Common.Audit;
using ChurchKid.Common.Resources;
using ChurchKid.Data.Entities.Miscellaneous;
using System;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;

namespace ChurchKid.Data.Seed
{
    public class EducationalLevels : ISeeder
    {

        private DatabaseConnection connection;

        public EducationalLevels(DatabaseConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.EducationalLevels;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            try
            {
                if (!connection.EducationalLevels.Any())
                {
                    var administratorId = 0;
                    var administrator = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCultureIgnoreCase));
                    if (administrator != null)
                        administratorId = administrator.UserId;
                
                    var data = XElement.Parse(SeedData);
                    var educationalLevels = from educationalLevel in data.Elements("educationalLevel")
                                            select new EducationalLevel()
                                            {
                                                Name = (string)educationalLevel.Element("name"),
                                                Description = (string)educationalLevel.Element("description"),
                                                CreatedById = administratorId
                                            };

                    if (educationalLevels.Any())
                    {
                        var module = connection.ApplicationModules.FirstOrDefault(m => "Educational Levels".Equals(m.Name, StringComparison.InvariantCultureIgnoreCase));

                        using (var transaction = new TransactionScope())
                        {
                            var persistedEducationalLevels = connection.EducationalLevels.AddRange(educationalLevels);
                            connection.SaveChanges();

                            if (administrator != null &&
                                module != null)
                            {
                                foreach (var persistedEducationalLevel in persistedEducationalLevels)
                                    connection.LogAction(administrator, module, UserActions.Add, persistedEducationalLevel.EducationalLevelId,
                                                        string.Format(ApplicationStrings.msgSeededDataSpecific, persistedEducationalLevel.Name, ApplicationStrings.dataEducationalLevels.ToLower()));
                            }

                            connection.SaveChanges();
                            transaction.Complete();
                            Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataEducationalLevels));
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
