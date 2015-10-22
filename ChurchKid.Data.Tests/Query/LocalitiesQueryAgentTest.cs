using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChurchKid.Data.Query;
using System.Collections.Generic;
using ChurchKid.Data.Entities.Geographic;
using ChurchKid.Data.Connections;
using MySql.Data.MySqlClient;

namespace ChurchKid.Data.Tests.Query
{
    [TestClass]
    public class LocalitiesQueryAgentTest
    {
        [TestMethod,
        TestCategory("Query")]
        public void LocalitiesQueryAgentShouldReturnReferences()
        {
            using (var connection = new FTTConnection())
            {
                var queryAgent = new LocalitiesQueryAgent(connection);
                var references = queryAgent.GetReferences();

                var countries = references.GetType().GetProperty("Countries").GetValue(references, null) as ICollection<Country>;
                Assert.IsTrue(countries.Any());

                var islands = references.GetType().GetProperty("Islands").GetValue(references, null) as ICollection<Island>;
                Assert.IsTrue(islands.Any());

                var regions = references.GetType().GetProperty("Regions").GetValue(references, null) as ICollection<Region>;
                Assert.IsTrue(regions.Any());

                var localityGroups = references.GetType().GetProperty("LocalityGroups").GetValue(references, null) as ICollection<LocalityGroup>;
                Assert.IsTrue(localityGroups.Any());
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void LocalitiesQueryAgentShouldReturnAllRecords()
        {
            using (var connection = new FTTConnection())
            {
                var persistedLocalities = connection.Localities.ToList();
                Assert.IsTrue(persistedLocalities.Any());

                var queryAgent = new LocalitiesQueryAgent(connection);
                var retrievedLocalities = queryAgent.GetAll();
                Assert.IsNotNull(retrievedLocalities);
                Assert.IsTrue(retrievedLocalities.Any());
                Assert.AreEqual(persistedLocalities.Count, retrievedLocalities.Count);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void LocalitiesQueryAgentShouldReturnSpecificRecord()
        {
            using (var connection = new FTTConnection())
            {
                var persistedLocality = connection.Localities.FirstOrDefault(l => "Malabon".Equals(l.Name, StringComparison.InvariantCultureIgnoreCase));
                Assert.IsNotNull(persistedLocality);

                var queryAgent = new LocalitiesQueryAgent(connection);
                var retrievedLocality = queryAgent.Get(persistedLocality.LocalityId);
                Assert.IsNotNull(retrievedLocality);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void LocalitiesQueryAgentShouldReturnSpecificRecords()
        {
            using (var connection = new FTTConnection())
            {
                var persistedLocalities = connection.Localities
                                                    .Where(l => l.Name.StartsWith("M"))
                                                    .ToList();

                Assert.IsTrue(persistedLocalities.Any());

                var queryAgent = new LocalitiesQueryAgent(connection);
                var retrievedLocalities = queryAgent.Get("localities.Name LIKE 'M%'", "localities.Name");
                Assert.IsNotNull(retrievedLocalities);
                Assert.IsTrue(retrievedLocalities.Any());
                Assert.AreEqual(persistedLocalities.Count, retrievedLocalities.Count);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void LocalitiesQueryAgentShouldReturnSpecificRecordsUsingParameters()
        {
            using (var connection = new FTTConnection())
            {
                var persistedLocalities = connection.Localities
                                                    .Where(l => l.Name.StartsWith("M"))
                                                    .ToList();

                Assert.IsTrue(persistedLocalities.Any());

                var queryAgent = new LocalitiesQueryAgent(connection);
                var retrievedLocalities = queryAgent.Get("localities.Name LIKE @NameFilter",
                                                         new MySqlParameter("@NameFilter", "M%"));
                Assert.IsNotNull(retrievedLocalities);
                Assert.IsTrue(retrievedLocalities.Any());
                Assert.AreEqual(persistedLocalities.Count, retrievedLocalities.Count);
            }
        }

    }
}
