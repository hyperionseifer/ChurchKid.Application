using ChurchKid.Data.Entities.Geographic;
using System.Collections.Generic;
using System.Linq;

namespace ChurchKid.Data.Query
{
    public class LocalitiesQueryAgent : IQueryAgent<Locality>, IReferencedQueryAgent
    {

        private DatabaseConnection connection;

        private string commandText;

        public LocalitiesQueryAgent(DatabaseConnection connection)
        {
            this.connection = connection;
            this.commandText = Properties.Resources.SqlGetAllLocalities;
        }

        public dynamic GetReferences()
        {
            var countriesQueryAgent = new CountriesQueryAgent(connection);
            var islandsQueryAgent = new IslandsQueryAgent(connection);
            var regionsQueryAgent = new RegionsQueryAgent(connection);
            var localityGroupsQueryAgent = new LocalityGroupsQueryAgent(connection);

            var countries = countriesQueryAgent.GetAll();
            var islands = islandsQueryAgent.GetAll();
            var regions = regionsQueryAgent.GetAll();
            var localityGroups = localityGroupsQueryAgent.GetAll();

            return new
            {
                Countries = countries,
                Islands = islands,
                Regions = regions,
                LocalityGroups = localityGroups
            };
        }

        public ICollection<Locality> Get(string filter)
        {
            return Get(filter, string.Empty);
        }

        public ICollection<Locality> Get(string filter, string sort)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}{1}{2}",
                                    commandText,
                                    !string.IsNullOrEmpty(filter.Trim()) ? "\nWHERE " + filter : string.Empty,
                                    !string.IsNullOrEmpty(sort.Trim()) ? "\nORDER BY " + sort : string.Empty);

            return connection.Database.SqlQuery<Locality>(sql).ToList();
        }

        public ICollection<Locality> Get(string filter, params System.Data.Common.DbParameter[] parameters)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\nWHERE {1}",
                                    commandText,
                                    filter);

            return connection.Database.SqlQuery<Locality>(sql, parameters).ToList();
        }

        public Locality Get(int id)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\nWHERE localities.LocalityId = {1}",
                                    commandText,
                                    id);

            return connection.Database.SqlQuery<Locality>(sql).FirstOrDefault();
        }

        public ICollection<Locality> GetAll()
        {
            return GetAll(string.Empty);
        }

        public ICollection<Locality> GetAll(string sort)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\nORDER BY {1}",
                                    commandText,
                                    string.IsNullOrEmpty(sort.Trim()) ? "localities.Name" : sort);

            return connection.Database.SqlQuery<Locality>(sql).ToList();
        }
    }
}
