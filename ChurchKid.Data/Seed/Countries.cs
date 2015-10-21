using ChurchKid.Common;
using ChurchKid.Common.Audit;
using ChurchKid.Common.Resources;
using ChurchKid.Data.Entities.Geographic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;

namespace ChurchKid.Data.Seed
{
    public class Countries : ISeeder
    {

        private DatabaseConnection connection;

        public Countries(DatabaseConnection connection)
        {
            this.connection = connection;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            if (!connection.Countries.Any())
            {
                try
                {
                    var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
                    var countries = new List<Country>();
                    var administratorId = 0;
                    var administrator = connection.ApplicationUsers.FirstOrDefault(u => "churchkid".Equals(u.Username, StringComparison.InvariantCultureIgnoreCase));
                    if (administrator != null)
                        administratorId = administrator.UserId;

                    foreach (var culture in cultures)
                    {
                        if (!string.IsNullOrEmpty(culture.Name.Trim()))
                        {
                            try
                            {
                                RegionInfo region = new RegionInfo(culture.Name);
                                if (!countries.Any(c => region.EnglishName.Equals(c.Name, StringComparison.InvariantCultureIgnoreCase))) 
                                    countries.Add(new Country() 
                                    {
                                        Name = region.EnglishName,
                                        CreatedById = administratorId
                                    });
                            }
                            catch { }
                        }
                    }

                    var additionalCountries = new string[] 
                    {
                        "Afghanistan", 
                        "Pakistan", 
                        "Liberia", 
                        "Somalia"
                    };

                    foreach (var additionalCountry in additionalCountries)
                    {
                        if (!countries.Any(c => additionalCountry.Equals(c.Name, StringComparison.InvariantCultureIgnoreCase))) 
                            countries.Add(new Country()
                            {
                               Name = additionalCountry,
                               CreatedById = administratorId
                            });
                    }

                    var module = connection.ApplicationModules.FirstOrDefault(m => "Countries".Equals(m.Name, StringComparison.InvariantCultureIgnoreCase));
                    
                    using (var transaction = new TransactionScope())
                    {
                        var persistedCountries = connection.Countries.AddRange(from country in countries
                                                                               orderby country.Name
                                                                               select country);
                        connection.SaveChanges();
                        
                        if (administrator != null &&
                            module != null)
                        {
                            foreach (var persistedCountry in persistedCountries)
                                connection.LogAction(administrator, module, UserActions.Add, persistedCountry.CountryId,
                                                     string.Format(ApplicationStrings.msgSeededDataSpecific, persistedCountry.Name, ApplicationStrings.dataCountries.ToLower()));

                        }

                        connection.SaveChanges();
                        transaction.Complete();
                        Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataCountries));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Write(ex);
                }
            }
        }

    }
}
