using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChurchKid.Data.Connections;
using ChurchKid.Data.Query;
using System.Collections.Generic;
using ChurchKid.Data.Entities.Geographic;
using MySql.Data.MySqlClient;

namespace ChurchKid.Data.Tests.Query
{
    [TestClass]
    public class RegionsQueryAgentTest
    {
        [TestMethod,
        TestCategory("Query")]
        public void RegionsQueryAgentShouldReturnReferences()
        {
            using (var connection = new FTTConnection())
            {
                var queryAgent = new RegionsQueryAgent(connection);
                var references = queryAgent.GetReferences();

                var countries = references.GetType().GetProperty("Countries").GetValue(references, null) as ICollection<Country>;
                Assert.IsTrue(countries.Any());

                var islands = references.GetType().GetProperty("Islands").GetValue(references, null) as ICollection<Island>;
                Assert.IsTrue(islands.Any());
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void RegionsQueryAgentShouldReturnAllRecords()
        {
            using (var connection = new FTTConnection())
            {
                var persistedRegions = connection.Regions.ToList();
                Assert.IsTrue(persistedRegions.Any());

                var queryAgent = new RegionsQueryAgent(connection);
                var retrievedRegions = queryAgent.GetAll();
                Assert.IsNotNull(retrievedRegions);
                Assert.IsTrue(retrievedRegions.Any());
                Assert.AreEqual(persistedRegions.Count, retrievedRegions.Count);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void RegionsQueryAgentShouldReturnSpecificRecord()
        {
            using (var connection = new FTTConnection())
            {
                var persistedRegion = connection.Regions.FirstOrDefault(r => "NCR".Equals(r.Name, StringComparison.InvariantCultureIgnoreCase));
                Assert.IsNotNull(persistedRegion);

                var queryAgent = new RegionsQueryAgent(connection);
                var retrievedRegion = queryAgent.Get(persistedRegion.RegionId);
                Assert.IsNotNull(retrievedRegion);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void RegionsQueryAgentShouldReturnSpecificRecords()
        {
            using (var connection = new FTTConnection())
            {
                var persistedRegions = connection.Regions
                                                 .Where(r => r.Name.StartsWith("Region"))
                                                 .ToList();

                Assert.IsTrue(persistedRegions.Any());

                var queryAgent = new RegionsQueryAgent(connection);
                var retrievedRegions = queryAgent.Get("regions.Name LIKE 'Region%'", "regions.Name");
                Assert.IsNotNull(retrievedRegions);
                Assert.IsTrue(retrievedRegions.Any());
                Assert.AreEqual(persistedRegions.Count, retrievedRegions.Count);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void RegionsQueryAgentShouldReturnSpecificRecordsUsingParameters()
        {
            using (var connection = new FTTConnection())
            {
                var persistedRegions = connection.Regions
                                                 .Where(r => r.Name.StartsWith("Region"))
                                                 .ToList();

                Assert.IsTrue(persistedRegions.Any());

                var queryAgent = new RegionsQueryAgent(connection);
                var retrievedRegions = queryAgent.Get("regions.Name LIKE @NameFilter",
                                                       new MySqlParameter("@NameFilter", "Region%"));
                Assert.IsNotNull(retrievedRegions);
                Assert.IsTrue(retrievedRegions.Any());
                Assert.AreEqual(persistedRegions.Count, retrievedRegions.Count);
            }
        }

    }
}
