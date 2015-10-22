using ChurchKid.Data.Entities.Geographic;
using System.Collections.Generic;
using System.Linq;

namespace ChurchKid.Data.Query
{
    public class IslandsQueryAgent : IQueryAgent<Island>, IReferencedQueryAgent
    {

        private DatabaseConnection connection;

        private string commandText;

        public IslandsQueryAgent(DatabaseConnection connection)
        {
            this.connection = connection;
            this.commandText = Properties.Resources.SqlGetAllIslands;
        }

        public dynamic GetReferences()
        {
            var countriesQueryAgent = new CountriesQueryAgent(connection);
            var countries = countriesQueryAgent.GetAll();

            return new
            {
                Countries = countries
            };
        }

        public ICollection<Island> Get(string filter)
        {
            return Get(filter, string.Empty);
        }

        public ICollection<Island> Get(string filter, string sort)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}{1}{2}",
                                    commandText,
                                    !string.IsNullOrEmpty(filter.Trim()) ? "\nWHERE " + filter : string.Empty,
                                    !string.IsNullOrEmpty(sort.Trim()) ? "\nORDER BY " + sort : string.Empty);

            return connection.Database.SqlQuery<Island>(sql).ToList();
        }

        public ICollection<Island> Get(string filter, params System.Data.Common.DbParameter[] parameters)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\nWHERE {1}",
                                    commandText,
                                    filter);

            return connection.Database.SqlQuery<Island>(sql, parameters).ToList();
        }

        public Island Get(int id)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\nWHERE islands.IslandId = {1}",
                                    commandText,
                                    id);

            return connection.Database.SqlQuery<Island>(sql).FirstOrDefault();
        }

        public ICollection<Island> GetAll()
        {
            return GetAll(string.Empty);
        }

        public ICollection<Island> GetAll(string sort)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\nORDER BY {1}",
                                    commandText,
                                    string.IsNullOrEmpty(sort.Trim()) ? "islands.Name" : sort);

            return connection.Database.SqlQuery<Island>(sql).ToList();
        }
    }
}
