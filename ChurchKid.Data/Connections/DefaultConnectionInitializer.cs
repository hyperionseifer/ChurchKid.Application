using System.Data.Entity;

namespace ChurchKid.Data.Connections
{
    public class DefaultConnectionInitializer : CreateDatabaseIfNotExists<DefaultConnection>
    {

        protected override void Seed(DefaultConnection context)
        {
            base.Seed(context);

            foreach (var seeder in context.Seeders)
                seeder.Seed();
        }
    }
}
