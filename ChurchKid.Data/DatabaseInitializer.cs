using System.Data.Entity;

namespace ChurchKid.Data
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<DatabaseConnection>
    {

        protected override void Seed(DatabaseConnection context)
        {
            base.Seed(context);

            foreach (var seeder in context.Seeders)
                seeder.Seed();
        }

    }
}
