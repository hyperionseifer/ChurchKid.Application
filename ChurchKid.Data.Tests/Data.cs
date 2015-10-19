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
            }
        }

    }
}
