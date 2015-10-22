using ChurchKid.Data.Entities.Geographic;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace ChurchKid.Data.Query
{
    public class CountriesQueryAgent : IQueryAgent<Country>
    {

        private DatabaseConnection connection;

        private string commandText;

        public CountriesQueryAgent(DatabaseConnection connection)
        {
            this.connection = connection;
            this.commandText = Properties.Resources.SqlGetAllCountries;
        }

        public ICollection<Country> Get(string filter, params DbParameter[] parameters)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\nWHERE {1}",
                                    commandText,
                                    filter);

            return connection.Database.SqlQuery<Country>(sql, parameters).ToList();
        }

        public ICollection<Country> Get(string filter)
        {
            return Get(filter, string.Empty);
        }

        public ICollection<Country> Get(string filter, string sort)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}{1}{2}",
                                    commandText,
                                    !string.IsNullOrEmpty(filter.Trim()) ? "\nWHERE " + filter : string.Empty,
                                    !string.IsNullOrEmpty(sort.Trim()) ? "ORDER BY " + sort : string.Empty);

            return connection.Database.SqlQuery<Country>(sql).ToList();
        }

        public Country Get(int id)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\nWHERE CountryId = @CountryId",
                                    commandText);

            return connection.Database
                             .SqlQuery<Country>(sql, new MySqlParameter("@CountryId", id))
                             .FirstOrDefault();
        }

        public ICollection<Country> GetAll()
        {
            return GetAll(string.Empty);
        }

        public ICollection<Country> GetAll(string sort)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\nORDER BY {1}",
                                    commandText,
                                    string.IsNullOrEmpty(sort.Trim())? "Name" : sort);

            return connection.Database.SqlQuery<Country>(sql).ToList();
        }

    }
}
