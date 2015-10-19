using ChurchKid.Data.Configuration;
using System.Data.Entity;

namespace ChurchKid.Data.Connections
{
    public class DefaultConnection : DatabaseConnection
    {

        public DefaultConnection()
            : base(ConnectionStringConfiguration.DefaultConnectionString.Name)
        {
            Database.SetInitializer<DefaultConnection>(new DefaultConnectionInitializer());
        }

    }
}
