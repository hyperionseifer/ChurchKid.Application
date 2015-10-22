using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChurchKid.Data.Connections;
using ChurchKid.Data.Query;
using MySql.Data.MySqlClient;

namespace ChurchKid.Data.Tests.Query
{
    [TestClass]
    public class CountriesQueryAgentTest
    {
        [TestMethod,
        TestCategory("Query")]
        public void CountriesQueryAgentShouldReturnAllRecords()
        {
            using (var connection = new FTTConnection())
            {
                var persistedCountries = connection.Countries.ToList();
                Assert.IsTrue(persistedCountries.Any());

                var queryAgent = new CountriesQueryAgent(connection);
                var retrievedCountries = queryAgent.GetAll();
                Assert.IsNotNull(retrievedCountries);
                Assert.IsTrue(retrievedCountries.Any());
                Assert.AreEqual(persistedCountries.Count, retrievedCountries.Count);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void CountriesQueryAgentShouldReturnSpecificRecord()
        {
            using (var connection = new FTTConnection())
            {
                var persistedCountry = connection.Countries.FirstOrDefault(c => "Philippines".Equals(c.Name, StringComparison.InvariantCultureIgnoreCase));
                Assert.IsNotNull(persistedCountry);

                var queryAgent = new CountriesQueryAgent(connection);
                var retrievedCountry = queryAgent.Get(persistedCountry.CountryId);
                Assert.IsNotNull(retrievedCountry);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void CountriesQueryAgentShouldReturnSpecificRecords()
        {
            using (var connection = new FTTConnection())
            {
                var persistedCountries = connection.Countries
                                                   .Where(c => c.Name.StartsWith("P"))
                                                   .ToList();

                Assert.IsTrue(persistedCountries.Any());

                var queryAgent = new CountriesQueryAgent(connection);
                var retrievedCountries = queryAgent.Get("Name LIKE 'P%'", "Name");
                Assert.IsNotNull(retrievedCountries);
                Assert.IsTrue(retrievedCountries.Any());
                Assert.AreEqual(persistedCountries.Count, retrievedCountries.Count);
            }
        }

        [TestMethod,
        TestCategory("Query")]
        public void CountriesQueryAgentShouldReturnSpecificRecordsUsingParameters()
        {
            using (var connection = new FTTConnection())
            {
                var persistedCountries = connection.Countries
                                                   .Where(c => c.Name.StartsWith("P"))
                                                   .ToList();

                Assert.IsTrue(persistedCountries.Any());

                var queryAgent = new CountriesQueryAgent(connection);
                var retrievedCountries = queryAgent.Get("Name LIKE @NameFilter", 
                                                        new MySqlParameter("@NameFilter", "P%"));
                Assert.IsNotNull(retrievedCountries);
                Assert.IsTrue(retrievedCountries.Any());
                Assert.AreEqual(persistedCountries.Count, retrievedCountries.Count);
            }
        }

    }
}
