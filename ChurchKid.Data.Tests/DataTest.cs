using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChurchKid.Data.Connections;

namespace ChurchKid.Data.Tests
{
    [TestClass]
    public class DataTest
    {
        [TestMethod, 
        TestCategory("Data")]
        public void CreateDatabaseIfNotExistsAndSeedData()
        {
            using (var connection = new FTTConnection())
            {
                var roles = connection.Roles.ToList();
                Assert.IsTrue(roles.Any());

                var users = connection.ApplicationUsers.ToList();
                Assert.IsTrue(users.Any());

                var churchKid = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCulture));
                Assert.IsNotNull(churchKid);
                Assert.IsTrue(churchKid.IsAdministrator);

                var moduleGroups = connection.ApplicationModuleGroups.ToList();
                Assert.IsTrue(moduleGroups.Any());

                var modules = connection.ApplicationModules.ToList();
                Assert.IsTrue(modules.Any());

                var countries = connection.Countries.ToList();
                Assert.IsTrue(countries.Any());

                var islands = connection.Islands.ToList();
                Assert.IsTrue(islands.Any());

                var regions = connection.Regions.ToList();
                Assert.IsTrue(regions.Any());

                var localityGroups = connection.LocalityGroups.ToList();
                Assert.IsTrue(localityGroups.Any());

                var localities = connection.Localities.ToList();
                Assert.IsTrue(localities.Any());

                var clusters = connection.Clusters.ToList();
                Assert.IsTrue(clusters.Any());

                var districts = connection.Districts.ToList();
                Assert.IsTrue(districts.Any());

                var educationalLevels = connection.EducationalLevels.ToList();
                Assert.IsTrue(educationalLevels.Any());

                var trainingCenters = connection.TrainingCenters.ToList();
                Assert.IsTrue(trainingCenters.Any());

                var trainingLevels = connection.TrainingLevels.ToList();
                Assert.IsTrue(trainingLevels.Any());
            }
        }

    }
}
