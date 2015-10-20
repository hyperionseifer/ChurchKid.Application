using ChurchKid.Common;
using ChurchKid.Data.Configuration;
using ChurchKid.Data.Entities.Audit;
using ChurchKid.Data.Entities.Geographic;
using ChurchKid.Data.Entities.Miscellaneous;
using ChurchKid.Data.Entities.SaintProfile;
using ChurchKid.Data.Entities.UserProfile;
using ChurchKid.Data.Seed;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ChurchKid.Data
{
    public class DatabaseConnection : DbContext
    {

        public DatabaseConnection() :
            base(ConnectionStringConfiguration.DefaultConnectionString.ConnectionString)
        {
            OnSeed();
        }

        public DatabaseConnection(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            OnSeed();
        }


        static DatabaseConnection()
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
        }

        public ICollection<ISeeder> Seeders { get; set; }

        protected virtual void OnSeed()
        {
            
            if (Seeders == null)
                Seeders = new List<ISeeder>();

            Seeders.Add(new RolesAndPrivileges(this));
            Seeders.Add(new Users(this));
            Seeders.Add(new ApplicationModuleGroups(this));
            Seeders.Add(new ApplicationModules(this));
            Seeders.Add(new Countries(this));

            Database.SetInitializer<DatabaseConnection>(new DatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #region Audit

        public virtual DbSet<ApplicationModule> ApplicationModules { get; set; }

        public virtual DbSet<ApplicationModuleGroup> ApplicationModuleGroups { get; set; }

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        #endregion

        #region Geographic

        public virtual DbSet<Cluster> Clusters { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<District> Districts { get; set; }

        public virtual DbSet<Island> Islands { get; set; }

        public virtual DbSet<Locality> Localities { get; set; }

        public virtual DbSet<LocalityGroup> LocalityGroups { get; set; }

        public virtual DbSet<Region> Regions { get; set; }

        #endregion

        #region Miscellaneous

        public virtual DbSet<EducationalLevel> EducationalLevels { get; set; }

        #endregion

        #region SaintProfile

        public virtual DbSet<Saint> Saints { get; set; }

        public virtual DbSet<SaintEducation> SaintEducations { get; set; }

        #endregion

        #region UserProfile

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public virtual DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        public virtual DbSet<Privilege> Privileges { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<RolePrivilege> RolePrivileges { get; set; }

        #endregion

        public void LogAction(ApplicationUser user, ApplicationModule module, string action, string details)
        {
            LogAction(user, module, action, details, string.Empty);
        }

        public void LogAction(ApplicationUser user, ApplicationModule module, string action, string details, string performedAt)
        {
            var machineIpOrHostName = (string.IsNullOrEmpty(performedAt) ? Environment.MachineName : performedAt);
            var userAction = (!UserActions.IsValidAction(action) ? UserActions.Unknown : action);
            AuditLogs.Add(new AuditLog()
            {
                Action = userAction,
                ApplicationModuleId = module.ApplicationModuleId,
                UserId = user.UserId,
                Details = details,
                PerformedAt = machineIpOrHostName
            });

            SaveChanges();
        }

    }

}
