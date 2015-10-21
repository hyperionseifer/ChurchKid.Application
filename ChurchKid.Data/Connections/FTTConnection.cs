using ChurchKid.Data.Configuration;
using ChurchKid.Data.Entities.Training;
using ChurchKid.Data.Seed;
using System.Data.Entity;

namespace ChurchKid.Data.Connections
{
    public class FTTConnection : DatabaseConnection
    {

        public FTTConnection() 
            : base(ConnectionStringConfiguration.FTTConnectionString.Name)
        {
            Seeders.Add(new TrainingCenters(this));
            Seeders.Add(new TrainingLevels(this));

            Database.SetInitializer<FTTConnection>(new FTTConnectionInitializer());
        }

        #region Training

        public virtual DbSet<Trainee> Trainees { get; set; }

        public virtual DbSet<TrainingCenter> TrainingCenters { get; set; }

        public virtual DbSet<TrainingLevel> TrainingLevels { get; set; }

        public virtual DbSet<Training> Trainings { get; set; }

        public virtual DbSet<TrainingClass> TrainingClasses { get; set; }

        public virtual DbSet<TrainingAttendee> TrainingAttendees { get; set; }

        #endregion

    }
}
