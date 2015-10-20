using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChurchKid.Data.Connections;

namespace ChurchKid.Data.Tests
{
    [TestClass]
    public class Data
    {
        [TestMethod]
        public void CreateDatabaseIfNotExistsAndSeedData()
        {
            using (var connection = new FTTConnection())
            {
                var roles = connection.Roles.ToList();
                Assert.IsTrue(roles.Count > 0);

                var users = connection.ApplicationUsers.ToList();
                Assert.IsTrue(users.Count > 0);

                var churchKid = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCulture));
                Assert.IsNotNull(churchKid);
                Assert.IsTrue(churchKid.IsAdministrator);

                var moduleGroups = connection.ApplicationModuleGroups.ToList();
                Assert.IsTrue(moduleGroups.Count > 0);

                var modules = connection.ApplicationModules.ToList();
                Assert.IsTrue(modules.Count > 0);

                var countries = connection.Countries.ToList();
                Assert.IsTrue(countries.Count > 0);
            }
        }

    }
}
