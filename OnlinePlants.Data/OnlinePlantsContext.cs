using OnlinePlants.Model.BusinessModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace OnlinePlants.Data
{
    public class OnlinePlantsContext : DbContext
    {
        public OnlinePlantsContext() : base("name=OnlinePlants")
        {
        }
        public DbSet<UserType> tblUserType { get; set; }
        public DbSet<User> tblUser { get; set; }
        public DbSet<Registration> tblRegistration { get; set; }
        public DbSet<Category> tblCategory { get; set; }
        public DbSet<Payments> tblPayment { get; set; }
        public DbSet<MetaKeyword> tblMetaKeyword { get; set; }
        public DbSet<ErrorLog> tblErrorLog { get; set; }
        //public DbSet<JobAlert> tblJobAlert { get; set; }
        //public DbSet<JobApplied> tblJobApplied { get; set; }
        //public DbSet<Package> tblPackage { get; set; }
        //public DbSet<Skills> tblSkills { get; set; }
        //public DbSet<SkillsCategory> tblSkillsCategory { get; set; }
        //public DbSet<Summary> tblSummary { get; set; }
        //public DbSet<JobType> tblJobType { get; set; }
        //public DbSet<Locations> tblLocations { get; set; }
        //public DbSet<Company> tblCompany { get; set; }
        //public DbSet<UserMessages> tblMessages { get; set; }
        //public DbSet<Payments> tblPayments { get; set; }
        //public DbSet<State> tblState { get; set; }
        //public DbSet<JobDays> tblJobDays { get; set; }
        //public DbSet<JobStatus> tblJobStatus { get; set; }
        //public DbSet<Contactus> tblContactus { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<Registration>().Property(a => a.CreatedDate).HasColumnType("datetime2");
        }
    }

    public class OnlinePlantsInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<OnlinePlantsContext>
    {
        protected override void Seed(OnlinePlantsContext context)
        {
            var userType = new List<UserType>
            {
                new UserType { TypeName = "Admin"},
                new UserType { TypeName = "Seller"}
            };

            userType.ForEach(a => context.tblUserType.Add(a));
            context.SaveChanges();
        }

    }
}
