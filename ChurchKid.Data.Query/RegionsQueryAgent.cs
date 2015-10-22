using ChurchKid.Data.Entities.Geographic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChurchKid.Data.Query
{
    public class RegionsQueryAgent : IQueryAgent<Region>, IReferencedQueryAgent
    {

        private DatabaseConnection connection;

        private string commandText;

        public RegionsQueryAgent(DatabaseConnection connection)
        {
            this.connection = connection;
            this.commandText = Properties.Resources.SqlGetAllRegions;
        }

        public dynamic GetReferences()
        {
            var countriesQueryAgent = new CountriesQueryAgent(connection);
            var islandsQueryAgent = new IslandsQueryAgent(connection);
            var countries = countriesQueryAgent.GetAll();
            var islands = islandsQueryAgent.GetAll();

            return new
            {
                Countries = countries,
                Islands = islands
            };
        }

        public ICollection<Region> Get(string filter)
        {
            return Get(filter, string.Empty);
        }

        public ICollection<Region> Get(string filter, string sort)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}{1}{2}",
                                    commandText,
                                    !string.IsNullOrEmpty(filter.Trim()) ? "\nWHERE " + filter : string.Empty,
                                    !string.IsNullOrEmpty(sort.Trim()) ? "\nORDER BY " + sort : string.Empty);

            return connection.Database.SqlQuery<Region>(sql).ToList();
        }

        public ICollection<Region> Get(string filter, params System.Data.Common.DbParameter[] parameters)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\nWHERE {1}",
                                    commandText,
                                    filter);

            return connection.Database.SqlQuery<Region>(sql, parameters).ToList();
        }

        public Region Get(int id)
        {
            if (connection == null)
                return null;

            var sql = string.Format(commandText + "\nWHERE regions.RegionId = {0}",
                                    id);

            return connection.Database.SqlQuery<Region>(sql).FirstOrDefault();
        }

        public ICollection<Region> GetAll()
        {
            return GetAll(string.Empty);
        }

        public ICollection<Region> GetAll(string sort)
        {
            if (connection == null)
                return null;

            var sql = string.Format(commandText + "\nORDER BY {0}",
                                    string.IsNullOrEmpty(sort.Trim()) ? "regions.Name" : sort);

            return connection.Database.SqlQuery<Region>(sql).ToList();
        }
    }
}
