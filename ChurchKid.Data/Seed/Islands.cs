using ChurchKid.Common;
using ChurchKid.Common.Audit;
using ChurchKid.Common.Resources;
using ChurchKid.Data.Entities.Geographic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace ChurchKid.Data.Seed
{
    public class Islands : ISeeder
    {

        private DatabaseConnection connection;

        public Islands(DatabaseConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.Islands;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            try
            {
                if (!connection.Islands.Any())
                {
                    var data = XElement.Parse(SeedData);
                    var islands = from island in data.Elements("island")
                                  select new
                                  {
                                      Country = (string)island.Element("country"),
                                      Name = (string)island.Element("name")
                                  };

                    if (islands.Any())
                    {
                        var persistedCountries = connection.Countries.ToList();
                        var administratorId = 0;
                        var administrator = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCultureIgnoreCase));
                        if (administrator != null)
                            administratorId = administrator.UserId;

                        var module = connection.ApplicationModules.FirstOrDefault(m => "Islands".Equals(m.Name, StringComparison.InvariantCultureIgnoreCase));

                        using (var transaction = new TransactionScope())
                        {
                            foreach (var island in islands)
                            {
                                var country = persistedCountries.FirstOrDefault(c => island.Country.Equals(c.Name, StringComparison.InvariantCultureIgnoreCase));
                                
                                var persistedIsland = connection.Islands.Add(new Island()
                                {
                                    CountryId = country.CountryId,
                                    Name = island.Name,
                                    CreatedById = administratorId
                                });

                                connection.SaveChanges();

                                if (administrator != null &&
                                    module != null)
                                    connection.LogAction(administrator, module, UserActions.Add, persistedIsland.IslandId,
                                                         string.Format(ApplicationStrings.msgSeededDataSpecific, persistedIsland.Name, ApplicationStrings.dataIslands.ToLower()));
                            }

                            connection.SaveChanges();
                            transaction.Complete();
                            Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataIslands));
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
