using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChurchKid.Data.Connections;
using ChurchKid.Data.Query;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using ChurchKid.Data.Entities.Geographic;

namespace ChurchKid.Data.Tests.Query
{
    [TestClass]
    public class IslandsQueryAgentTest
    {

        [TestMethod,
        TestCategory("Query")]
        public void IslandsQueryAgentShouldReturnReferences()
        {
            using (var connection = new FTTConnection())
            {
                var queryAgent = new IslandsQueryAgent(connection);
                var references = queryAgent.GetReferences();
                var countries = references.GetType().GetProperty("Countries").GetValue(references, null) as ICollection<Country>;
                Assert.IsTrue(countries.Any());
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void IslandsQueryAgentShouldReturnAllRecords()
        {
            using (var connection = new FTTConnection())
            {
                var persistedIslands = connection.Islands.ToList();
                Assert.IsTrue(persistedIslands.Any());

                var queryAgent = new IslandsQueryAgent(connection);
                var retrievedIslands = queryAgent.GetAll();
                Assert.IsNotNull(retrievedIslands);
                Assert.IsTrue(retrievedIslands.Any());
                Assert.AreEqual(persistedIslands.Count, retrievedIslands.Count);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void IslandsQueryAgentShouldReturnSpecificRecord()
        {
            using (var connection = new FTTConnection())
            {
                var persistedIsland = connection.Islands.FirstOrDefault(i => "Luzon".Equals(i.Name, StringComparison.InvariantCultureIgnoreCase));
                Assert.IsNotNull(persistedIsland);

                var queryAgent = new IslandsQueryAgent(connection);
                var retrievedIsland = queryAgent.Get(persistedIsland.IslandId);
                Assert.IsNotNull(retrievedIsland);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void IslandsQueryAgentShouldReturnSpecificRecords()
        {
            using (var connection = new FTTConnection())
            {
                var persistedIslands = connection.Islands
                                                 .Where(i => i.Name.StartsWith("L"))
                                                 .ToList();

                Assert.IsTrue(persistedIslands.Any());

                var queryAgent = new IslandsQueryAgent(connection);
                var retrievedIslands = queryAgent.Get("islands.Name LIKE 'L%'", "islands.Name");
                Assert.IsNotNull(retrievedIslands);
                Assert.IsTrue(retrievedIslands.Any());
                Assert.AreEqual(persistedIslands.Count, retrievedIslands.Count);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void IslandsQueryAgentShouldReturnSpecificRecordsUsingParameters()
        {
            using (var connection = new FTTConnection())
            {
                var persistedIslands = connection.Islands
                                                 .Where(i => i.Name.StartsWith("L"))
                                                 .ToList();

                Assert.IsTrue(persistedIslands.Any());

                var queryAgent = new IslandsQueryAgent(connection);
                var retrievedIslands = queryAgent.Get("islands.Name LIKE @NameFilter",
                                                       new MySqlParameter("@NameFilter", "L%"));
                Assert.IsNotNull(retrievedIslands);
                Assert.IsTrue(retrievedIslands.Any());
                Assert.AreEqual(persistedIslands.Count, retrievedIslands.Count);
            }
        }

    }
}
