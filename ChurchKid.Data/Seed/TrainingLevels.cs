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
    public class TrainingLevels : ISeeder
    {

        private FTTConnection connection;

        public TrainingLevels(FTTConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.TrainingLevels;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            try
            {
                if (!connection.TrainingLevels.Any())
                {
                    var administratorId = 0;
                    var administrator = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCultureIgnoreCase));
                    if (administrator != null)
                        administratorId = administrator.UserId;

                    var data = XElement.Parse(SeedData);

                    var trainingLevels = from trainingLevel in data.Elements("trainingLevel")
                                         select new TrainingLevel()
                                         {
                                             Name = (string)trainingLevel.Element("name"),
                                             Description = (string)trainingLevel.Element("description"),
                                             CreatedById = administratorId
                                         };
                    
                    if (trainingLevels.Any())
                    {
                        var module = connection.ApplicationModules.FirstOrDefault(m => "Training Levels".Equals(m.Name, StringComparison.InvariantCultureIgnoreCase));

                        using (var transaction = new TransactionScope())
                        {
                            var persistedTrainingLevels = connection.TrainingLevels.AddRange(trainingLevels);
                            connection.SaveChanges();

                            if (administrator != null &&
                                module != null)
                            {
                                foreach (var persistedTrainingLevel in persistedTrainingLevels)
                                    connection.LogAction(administrator, module, UserActions.Add, persistedTrainingLevel.TrainingLevelId,
                                                         string.Format(ApplicationStrings.msgSeededDataSpecific, persistedTrainingLevel.Name, ApplicationStrings.dataTrainingLevels.ToLower()));
                                
                                connection.SaveChanges();
                            }

                            transaction.Complete();
                            Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataTrainingLevels));
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
