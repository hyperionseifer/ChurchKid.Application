using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChurchKid.Data.Connections;
using ChurchKid.Data.Query;
using ChurchKid.Data.Entities.Geographic;
using MySql.Data.MySqlClient;

namespace ChurchKid.Data.Tests.Query
{
    [TestClass]
    public class LocalityGroupsQueryAgentTest
    {
        [TestMethod,
        TestCategory("Query")]
        public void LocalityGroupsQueryAgentShouldReturnReferences()
        {
            using (var connection = new FTTConnection())
            {
                var queryAgent = new LocalityGroupsQueryAgent(connection);
                var references = queryAgent.GetReferences();

                var countries = references.GetType().GetProperty("Countries").GetValue(references, null) as ICollection<Country>;
                Assert.IsTrue(countries.Any());

                var islands = references.GetType().GetProperty("Islands").GetValue(references, null) as ICollection<Island>;
                Assert.IsTrue(islands.Any());

                var regions = references.GetType().GetProperty("Regions").GetValue(references, null) as ICollection<Region>;
                Assert.IsTrue(regions.Any());
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void LocalityGroupsQueryAgentShouldReturnAllRecords()
        {
            using (var connection = new FTTConnection())
            {
                var persistedLocalityGroups = connection.LocalityGroups.ToList();
                Assert.IsTrue(persistedLocalityGroups.Any());

                var queryAgent = new LocalityGroupsQueryAgent(connection);
                var retrievedLocalityGroups = queryAgent.GetAll();
                Assert.IsNotNull(retrievedLocalityGroups);
                Assert.IsTrue(retrievedLocalityGroups.Any());
                Assert.AreEqual(persistedLocalityGroups.Count, retrievedLocalityGroups.Count);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void LocalityGroupsQueryAgentShouldReturnSpecificRecord()
        {
            using (var connection = new FTTConnection())
            {
                var persistedLocalityGroup = connection.LocalityGroups.FirstOrDefault(lg => "CAMANAVA".Equals(lg.Name, StringComparison.InvariantCultureIgnoreCase));
                Assert.IsNotNull(persistedLocalityGroup);

                var queryAgent = new LocalityGroupsQueryAgent(connection);
                var retrievedLocalityGroup = queryAgent.Get(persistedLocalityGroup.LocalityGroupId);
                Assert.IsNotNull(retrievedLocalityGroup);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void LocalityGroupsQueryAgentShouldReturnSpecificRecords()
        {
            using (var connection = new FTTConnection())
            {
                var persistedLocalityGroups = connection.LocalityGroups
                                                        .Where(lg => lg.Name.StartsWith("C"))
                                                        .ToList();

                Assert.IsTrue(persistedLocalityGroups.Any());

                var queryAgent = new LocalityGroupsQueryAgent(connection);
                var retrievedLocalityGroups = queryAgent.Get("localitygroups.Name LIKE 'C%'", "localitygroups.Name");
                Assert.IsNotNull(retrievedLocalityGroups);
                Assert.IsTrue(retrievedLocalityGroups.Any());
                Assert.AreEqual(persistedLocalityGroups.Count, retrievedLocalityGroups.Count);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void LocalityGroupsQueryAgentShouldReturnSpecificRecordsUsingParameters()
        {
            using (var connection = new FTTConnection())
            {
                var persistedLocalityGroups = connection.LocalityGroups
                                                        .Where(lg => lg.Name.StartsWith("C"))
                                                        .ToList();

                Assert.IsTrue(persistedLocalityGroups.Any());

                var queryAgent = new LocalityGroupsQueryAgent(connection);
                var retrievedLocalityGroups = queryAgent.Get("localitygroups.Name LIKE @NameFilter",
                                                             new MySqlParameter("@NameFilter", "C%"));
                Assert.IsNotNull(retrievedLocalityGroups);
                Assert.IsTrue(retrievedLocalityGroups.Any());
                Assert.AreEqual(persistedLocalityGroups.Count, retrievedLocalityGroups.Count);
            }
        }

    }
}
