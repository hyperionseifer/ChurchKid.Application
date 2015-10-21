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
    public class Districts : ISeeder
    {

        private DatabaseConnection connection;

        public Districts(DatabaseConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.Districts;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            try
            {
                if (!connection.Districts.Any())
                {
                    var data = XElement.Parse(SeedData);
                    var districts = from district in data.Elements("district")
                                    select new 
                                    {
                                        CountryName = (string)district.Element("country"),
                                        IslandName = (string)district.Element("island"),
                                        RegionName = (string)district.Element("region"),
                                        LocalityGroupName = (string)district.Element("group"),
                                        LocalityName = (string)district.Element("locality"),
                                        ClusterName = (string)district.Element("cluster"),
                                        Name = (string)district.Element("name"),
                                        Description = (string)district.Element("description")
                                    };

                    if (districts.Any())
                    {
                        var persistedLocalities = connection.Localities
                                                            .Include("Region")
                                                            .Include("Region.Country")
                                                            .ToList();

                        var localities = (from locality in persistedLocalities
                                          join localityGroup in connection.LocalityGroups on locality.LocalityGroupId equals localityGroup.LocalityGroupId into localityToGroup
                                          from localityGroup in localityToGroup.DefaultIfEmpty(new LocalityGroup())
                                          join island in connection.Islands on locality.Region.IslandId equals island.IslandId into regionToIsland
                                          from island in regionToIsland.DefaultIfEmpty(new Island())
                                          select new
                                          {
                                              LocalityId = locality.LocalityId,
                                              Name = locality.Name,
                                              CountryName = locality.Region.Country.Name,
                                              IslandName = island.Name,
                                              RegionName = locality.Region.Name,
                                              GroupName = localityGroup.Name
                                          }).ToList();

                        var clusters = connection.Clusters.ToList();

                        var administratorId = 0;
                        var administrator = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCultureIgnoreCase));
                        if (administrator != null)
                            administratorId = administrator.UserId;
                
                        var module = connection.ApplicationModules.FirstOrDefault(m => "Clusters".Equals(m.Name, StringComparison.InvariantCultureIgnoreCase));

                        using (var transaction = new TransactionScope())
                        {
                            foreach (var district in districts)
                            {
                                var locality = localities.FirstOrDefault(l => district.CountryName.Equals(l.CountryName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              district.IslandName.Equals(l.IslandName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              district.RegionName.Equals(l.RegionName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              district.LocalityGroupName.Equals(l.GroupName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              district.LocalityName.Equals(l.Name, StringComparison.InvariantCultureIgnoreCase));

                                var clusterId = 0;
                                var cluster = clusters.FirstOrDefault(c => district.ClusterName.Equals(c.Name, StringComparison.InvariantCultureIgnoreCase) &&
                                                                           c.LocalityId == locality.LocalityId);
                                if (cluster != null)
                                    clusterId = cluster.ClusterId;

                                var persistedDistrict = connection.Districts.Add(new District()
                                {
                                    Name = district.Name,
                                    ClusterId = clusterId,
                                    LocalityId = locality.LocalityId,
                                    Description = district.Description,
                                    CreatedById = administratorId
                                });

                                connection.SaveChanges();

                                if (administrator != null &&
                                   module != null)
                                    connection.LogAction(administrator, module, UserActions.Add, persistedDistrict.DistrictId,
                                                        string.Format(ApplicationStrings.msgSeededDataSpecific, persistedDistrict.Name, ApplicationStrings.dataDistricts.ToLower()));
                            }

                            connection.SaveChanges();
                            transaction.Complete();
                            Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataDistricts));
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
