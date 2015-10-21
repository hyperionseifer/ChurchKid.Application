using ChurchKid.Common;
using ChurchKid.Common.Audit;
using ChurchKid.Common.Resources;
using ChurchKid.Data.Connections;
using ChurchKid.Data.Entities.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace ChurchKid.Data.Seed
{
    public class TrainingCenters : ISeeder
    {

        private FTTConnection connection;

        public TrainingCenters(FTTConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.TrainingCenters;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            try
            {
                if (!connection.TrainingCenters.Any())
                {
                    var data = XElement.Parse(SeedData);
                    var trainingCenters = from trainingCenter in data.Elements("trainingCenter")
                                          select new
                                          {
                                              CountryName = (string)trainingCenter.Element("country"),
                                              RegionName = (string)trainingCenter.Element("region"),
                                              LocalityName = (string)trainingCenter.Element("locality"),
                                              Name = (string)trainingCenter.Element("name"),
                                              Address = (string)trainingCenter.Element("address"),
                                              ContactNo = (string)trainingCenter.Element("contactNo"),
                                              Email = (string)trainingCenter.Element("email")
                                          };

                    if (trainingCenters.Any())
                    {
                        var localities = connection.Localities
                                                   .Include("Region")
                                                   .Include("Region.Country")
                                                   .ToList();

                        var administratorId = 0;
                        var administrator = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCultureIgnoreCase));
                        if (administrator != null)
                            administratorId = administrator.UserId;

                        var module = connection.ApplicationModules.FirstOrDefault(m => "Training Centers".Equals(m.Name, StringComparison.InvariantCultureIgnoreCase));
                        
                        using (var transaction = new TransactionScope())
                        {
                            foreach (var trainingCenter in trainingCenters)
                            {
                                var locality = localities.FirstOrDefault(l => trainingCenter.CountryName.Equals(l.Region.Country.Name, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              trainingCenter.RegionName.Equals(l.Region.Name, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              trainingCenter.LocalityName.Equals(l.Name, StringComparison.InvariantCultureIgnoreCase));

                                var persistedTrainingCenter = connection.TrainingCenters.Add(new TrainingCenter()
                                {
                                    Name = trainingCenter.Name,
                                    LocalityId = locality.LocalityId,
                                    Address = trainingCenter.Address,
                                    ContactNo = trainingCenter.ContactNo,
                                    Email = trainingCenter.Email,
                                    CreatedById = administratorId
                                });

                                connection.SaveChanges();

                                if (administrator != null &&
                                    module != null)
                                    connection.LogAction(administrator, module, UserActions.Add, persistedTrainingCenter.TrainingCenterId,
                                                         string.Format(ApplicationStrings.msgSeededDataSpecific, persistedTrainingCenter.Name, ApplicationStrings.dataTrainingCenters));
                            }

                            connection.SaveChanges();
                            transaction.Complete();
                            Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataTrainingCenters));
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
