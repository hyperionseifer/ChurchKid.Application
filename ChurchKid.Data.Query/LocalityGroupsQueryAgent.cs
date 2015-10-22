using ChurchKid.Data.Entities.Geographic;
using System.Collections.Generic;
using System.Linq;

namespace ChurchKid.Data.Query
{
    public class LocalityGroupsQueryAgent : IQueryAgent<LocalityGroup>, IReferencedQueryAgent
    {

        private DatabaseConnection connection;

        private string commandText;

        public LocalityGroupsQueryAgent(DatabaseConnection connection)
        {
            this.connection = connection;
            this.commandText = Properties.Resources.SqlGetAllLocalityGroups;
        }

        public dynamic GetReferences()
        {
            var countriesQueryAgent = new CountriesQueryAgent(connection);
            var islandsQueryAgent = new IslandsQueryAgent(connection);
            var regionsQueryAgent = new RegionsQueryAgent(connection);

            var countries = countriesQueryAgent.GetAll();
            var islands = islandsQueryAgent.GetAll();
            var regions = regionsQueryAgent.GetAll();

            return new
            {
                Countries = countries,
                Islands = islands,
                Regions = regions
            };
        }

        public ICollection<LocalityGroup> Get(string filter)
        {
            return Get(filter, string.Empty);
        }

        public ICollection<LocalityGroup> Get(string filter, string sort)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}{1}{2}",
                                    commandText,
                                    !string.IsNullOrEmpty(filter.Trim()) ? "\nWHERE " + filter : string.Empty,
                                    !string.IsNullOrEmpty(sort.Trim()) ? "\nORDER BY " + sort : string.Empty);

            return connection.Database.SqlQuery<LocalityGroup>(sql).ToList();
        }

        public ICollection<LocalityGroup> Get(string filter, params System.Data.Common.DbParameter[] parameters)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\n WHERE {1}",
                                    commandText,
                                    filter);

            return connection.Database.SqlQuery<LocalityGroup>(sql, parameters).ToList();
        }

        public LocalityGroup Get(int id)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\nWHERE (localitygroups.LocalityGroupId = {1})",
                                    commandText,
                                    id);

            return connection.Database.SqlQuery<LocalityGroup>(sql).FirstOrDefault();
        }

        public ICollection<LocalityGroup> GetAll()
        {
            return GetAll(string.Empty);
        }

        public ICollection<LocalityGroup> GetAll(string sort)
        {
            if (connection == null)
                return null;

            var sql = string.Format("{0}\nORDER BY {1}",
                                    commandText,
                                    string.IsNullOrEmpty(sort.Trim()) ? "localitygroups.Name" : sort);

            return connection.Database.SqlQuery<LocalityGroup>(sql).ToList();
        }
    }
}
