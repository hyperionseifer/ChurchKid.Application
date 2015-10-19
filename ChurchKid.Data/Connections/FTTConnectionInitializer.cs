using System.Data.Entity;

namespace ChurchKid.Data.Connections
{
    public class FTTConnectionInitializer : CreateDatabaseIfNotExists<FTTConnection>
    {

        protected override void Seed(FTTConnection context)
        {
            base.Seed(context);

            foreach (var seeder in context.Seeders)
                seeder.Seed();
        }

    }
}
