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
    public class Localities : ISeeder
    {

        private DatabaseConnection connection;

        public Localities(DatabaseConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.Localities;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            try
            {
                if (!connection.Localities.Any())
                {
                    var data = XElement.Parse(SeedData);
                    var localities = from locality in data.Elements("locality")
                                     select new
                                     {
                                         CountryName = (string)locality.Element("country"),
                                         IslandName = (string)locality.Element("island"),
                                         RegionName = (string)locality.Element("region"),
                                         LocalityGroupName = (string)locality.Element("group"),
                                         Name = (string)locality.Element("name"),
                                         Description = (string)locality.Element("description")
                                     };

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

                    var localityGroups = connection.LocalityGroups.ToList();

                    var administratorId = 0;
                    var administrator = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCultureIgnoreCase));
                    if (administrator != null)
                        administratorId = administrator.UserId;

                    var module = connection.ApplicationModules.FirstOrDefault(m => "Localities".Equals(m.Name, StringComparison.InvariantCultureIgnoreCase));

                    using (var transaction = new TransactionScope())
                    {
                        foreach (var locality in localities)
                        {
                            var region = regions.FirstOrDefault(r => locality.CountryName.Equals(r.CountryName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                     locality.IslandName.Equals(r.IslandName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                     locality.RegionName.Equals(r.Name, StringComparison.InvariantCultureIgnoreCase));

                            var localityGroupId = 0;
                            var localityGroup = localityGroups.FirstOrDefault(lg => locality.LocalityGroupName.Equals(lg.Name, StringComparison.InvariantCultureIgnoreCase) &&
                                                                                    lg.RegionId == region.RegionId);
                            if (localityGroup != null)
                                localityGroupId = localityGroup.LocalityGroupId;

                            var persistedLocality = connection.Localities.Add(new Locality()
                            {
                                RegionId = region.RegionId,
                                LocalityGroupId = localityGroupId,
                                Name = locality.Name,
                                Description = locality.Description,
                                CreatedById = administratorId
                            });

                            connection.SaveChanges();

                            if (administrator != null &&
                                module != null)
                                connection.LogAction(administrator, module, UserActions.Add, persistedLocality.LocalityId,
                                                     string.Format(ApplicationStrings.msgSeededDataSpecific, persistedLocality.Name, ApplicationStrings.dataLocalities.ToLower()));
                        }

                        connection.SaveChanges();
                        transaction.Complete();
                        Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataLocalities));
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
