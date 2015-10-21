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
    public class Clusters : ISeeder
    {

        private DatabaseConnection connection;

        public Clusters(DatabaseConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.Clusters;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            try
            {
                if (!connection.Clusters.Any())
                {
                    var data = XElement.Parse(SeedData);
                    var clusters = from cluster in data.Elements("cluster")
                                   select new
                                   {
                                       CountryName = (string)cluster.Element("country"),
                                       IslandName = (string)cluster.Element("island"),
                                       RegionName = (string)cluster.Element("region"),
                                       LocalityGroupName = (string)cluster.Element("group"),
                                       LocalityName = (string)cluster.Element("locality"),
                                       Name = (string)cluster.Element("name"),
                                       Description = (string)cluster.Element("description")
                                   };

                    if (clusters.Any())
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

                        var administratorId = 0;
                        var administrator = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCultureIgnoreCase));
                        if (administrator != null)
                            administratorId = administrator.UserId;

                        var module = connection.ApplicationModules.FirstOrDefault(m => "Clusters".Equals(m.Name, StringComparison.InvariantCultureIgnoreCase));

                        using (var transaction = new TransactionScope())
                        {
                            foreach (var cluster in clusters)
                            {
                                var locality = localities.FirstOrDefault(l => cluster.CountryName.Equals(l.CountryName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              cluster.IslandName.Equals(l.IslandName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              cluster.RegionName.Equals(l.RegionName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              cluster.LocalityGroupName.Equals(l.GroupName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              cluster.LocalityName.Equals(l.Name, StringComparison.InvariantCultureIgnoreCase));
                                
                                var persistedCluster = connection.Clusters.Add(new Cluster()
                                {
                                    Name = cluster.Name,
                                    LocalityId = locality.LocalityId,
                                    Description = cluster.Description,
                                    CreatedById = administratorId
                                });

                                connection.SaveChanges();

                                if (administrator != null &&
                                    module != null)
                                    connection.LogAction(administrator, module, UserActions.Add, persistedCluster.ClusterId,
                                                         string.Format(ApplicationStrings.msgSeededDataSpecific, persistedCluster.Name, ApplicationStrings.dataClusters.ToLower()));
                            }

                            connection.SaveChanges();
                            transaction.Complete();
                            Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataClusters));
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
