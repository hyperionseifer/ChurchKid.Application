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
    public class Regions : ISeeder
    {

        private DatabaseConnection connection;

        public Regions(DatabaseConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.Regions;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            try
            {
                if (!connection.Regions.Any())
                {
                    var data = XElement.Parse(SeedData);
                    var regions = from region in data.Elements("region")
                                  select new
                                  {
                                      Country = (string)region.Element("country"),
                                      Island = (string)region.Element("island"),
                                      Name = (string)region.Element("name"),
                                      Description = (string)region.Element("description")
                                  };

                    if (regions.Any())
                    {
                        var persistedCountries = connection.Countries.ToList();
                        var persistedIslands = connection.Islands
                                                         .Include("Country").ToList();

                        var administratorId = 0;
                        var administrator = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCultureIgnoreCase));
                        if (administrator != null)
                            administratorId = administrator.UserId;

                        var module = connection.ApplicationModules.FirstOrDefault(m => "Regions".Equals(m.Name, StringComparison.InvariantCultureIgnoreCase));

                        using (var transaction = new TransactionScope())
                        {
                            foreach (var region in regions)
                            {
                                var country = persistedCountries.FirstOrDefault(c => region.Country.Equals(c.Name, StringComparison.InvariantCultureIgnoreCase));
                                var islandId = 0;
                                var island = persistedIslands.FirstOrDefault(i => (region.Country.Equals(i.Country.Name, StringComparison.InvariantCultureIgnoreCase) &&
                                                                                   region.Island.Equals(i.Name, StringComparison.InvariantCultureIgnoreCase)));

                                if (island != null)
                                    islandId = island.IslandId;

                                var persistedRegion = connection.Regions.Add(new Region()
                                {
                                   CountryId = country.CountryId,
                                   IslandId = islandId,
                                   Name = region.Name,
                                   Description = region.Description,
                                   CreatedById = administratorId
                                });

                                connection.SaveChanges();

                                if (administrator != null &&
                                    module != null)
                                    connection.LogAction(administrator, module, UserActions.Add, persistedRegion.RegionId,
                                                         string.Format(ApplicationStrings.msgSeededDataSpecific, persistedRegion.Name, ApplicationStrings.dataRegions.ToLower()));
                            }

                            connection.SaveChanges();
                            transaction.Complete();
                            Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataRegions));
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
