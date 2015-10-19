
namespace ChurchKid.Data.Seed
{
    public interface ISeeder
    {

        string SeedData { get; set; }

        void Seed();
    }
}
