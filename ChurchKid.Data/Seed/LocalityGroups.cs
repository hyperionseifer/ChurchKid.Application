using ChurchKid.Common;
using ChurchKid.Common.Audit;
using ChurchKid.Common.Resources;
using ChurchKid.Data.Entities.Geographic;
using System;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;

namespace ChurchKid.Data.Seed
{
    public class LocalityGroups : ISeeder
    {

        private DatabaseConnection connection;

        public LocalityGroups(DatabaseConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.LocalityGroups;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            try
            {
                
                if (!connection.LocalityGroups.Any())
                {
                    var data = XElement.Parse(SeedData);
                    var localityGroups = from localityGroup in data.Elements("localityGroup")
                                         select new
                                         {
                                             Country = (string)localityGroup.Element("country"),
                                             Island = (string)localityGroup.Element("island"),
                                             Region = (string)localityGroup.Element("region"),
                                             Name = (string)localityGroup.Element("name"),
                                             Description = (string)localityGroup.Element("description")
                                         };

                    if (localityGroups.Any())
                    {
                        var persistedRegions = connection.Regions
                                                         .Include("Country")
                                                         .ToList();

                        var regions = (from region in persistedRegions
                                      join island in connection.Islands on region.IslandId equals island.IslandId into regionToIsland
                                      from island in regionToIsland.DefaultIfEmpty(new Island())
                                      select new
                                      {
                                          RegionId = region.RegionId,
                                          CountryName = region.Country.Name,
                                          IslandName = island.Name,
                                          Name = region.Name
                                      }).ToList();

                        var administratorId = 0;
                        var administrator = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCultureIgnoreCase));
                        if (administrator != null)
                            administratorId = administrator.UserId;

                        var module = connection.ApplicationModules.FirstOrDefault(m => "Locality Groups".Equals(m.Name, StringComparison.InvariantCultureIgnoreCase));

                        using (var transaction = new TransactionScope())
                        {
                            foreach (var localityGroup in localityGroups)
                            {
                                var region = regions.FirstOrDefault(r => localityGroup.Country.Equals(r.CountryName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                         localityGroup.Island.Equals(r.IslandName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                         localityGroup.Region.Equals(r.Name, StringComparison.InvariantCultureIgnoreCase));

                                var persistedLocalityGroup = connection.LocalityGroups.Add(new LocalityGroup()
                                {
                                    RegionId = region.RegionId,
                                    Name = localityGroup.Name,
                                    Description = localityGroup.Description,
                                    CreatedById = administratorId
                                });

                                connection.SaveChanges();

                                if (administrator != null &&
                                    module != null)
                                    connection.LogAction(administrator, module, UserActions.Add, persistedLocalityGroup.LocalityGroupId,
                                                         string.Format(ApplicationStrings.msgSeededDataSpecific, persistedLocalityGroup.Name, ApplicationStrings.dataLocalityGroups.ToLower()));
                            }

                            connection.SaveChanges();
                            transaction.Complete();
                            Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataLocalityGroups));
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
