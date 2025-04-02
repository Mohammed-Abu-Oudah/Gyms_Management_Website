using GymApp.Configurations.Entities;
using GymApp.Credentials;
using GymApp.Data;
using GymApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace GymApp.ContextsAndFlunetAPIs
{
    public class DatabaseContext : IdentityDbContext<User> // To add the identity framework and use it, instead of using the DbContext we'll be using the IdentityDbContext.
    {

        public DatabaseContext(DbContextOptions options) : base(options)
        {}


        public DbSet<TrainingPlan> TrainingPlans { get; set; }
        public DbSet<Gym> Gym { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // this is the shared guid when I want to seed some users to make a conflict
            var guids = new GuidsSeederDefinition();

            // Here we define the Roles of the users, and we seed the roles. But, instead of jamming the roles here
            // what we are going to do is that we will but them in another class in the configurations folder.
            // we will call this class RoleConfigurations and we will add it in Entities subfolder.
            modelBuilder.ApplyConfiguration(new RoleConfigurations());
            modelBuilder.ApplyConfiguration(new UserTableConfigurations());
            modelBuilder.ApplyConfiguration(new GymTableConfigurations());
            modelBuilder.ApplyConfiguration(new TrainingPlanTableConfigurations());

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = guids.SupeAdminRuleGuid.ToString(),
                    UserId = guids.SuperAdminGuid.ToString()
                },
                new IdentityUserRole<string>
                {
                    RoleId = guids.GymAdminRuleGuid.ToString(),
                    UserId = guids.GymAdminGuid.ToString()
                }
                );

            /*// Properties definitions.
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasDefaultValue(Guid.NewGuid().ToString());

            modelBuilder.Entity<User>()
                .Property(u => u.UserName)
                .HasComputedColumnSql("[Email]");

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired(true);

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired(true);

            modelBuilder.Entity<User>()
                .Property(prop => prop.DisplayName)
                .HasComputedColumnSql("[UserFirstName] + ', ' + [UserMiddleName] + ', ' + [UserLastName]")
                .HasMaxLength(70)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(p => p.FullAddress)
                .HasComputedColumnSql("[ResidenceCountry] + '/' + [ResidenceCity] + '/' + [ResidenceStreet]")
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasOne(user => user.TrainingPlan)
                .WithOne(trainingPlan => trainingPlan.User)
                .HasForeignKey<TrainingPlan>(trainingPlan => trainingPlan.UserId)
                .IsRequired();*/

            /*modelBuilder.Entity<Gym>()
                .HasMany(gym => gym.Users)
                .WithOne(user => user.Gym)
                .HasForeignKey(user => user.GymId)
                .IsRequired(false);

            modelBuilder.Entity<Gym>()
                .Property(gym => gym.GymName)
                .IsRequired();*/


            // TrainingPlan model builder

            /*modelBuilder.Entity<TrainingPlan>()
                .HasKey(plan => plan.PlanId);*/

            /*********************************************************/
            // Database seeding.

            /*// Password hasher
            var hasher = new PasswordHasher<IdentityUser>();

            // For the data seeding we will only seed the SuperAdmin, which is only one and responsible for adding new admins to the database or deleting them.
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Email = "medo.doood2211@gmail.com",
                    IdNumber = "406959007",
                    PhoneNumber = "0592724313",
                    UserFirstName = "Mohammed",
                    UserMiddleName = "Farid",
                    UserLastName = "Abu-Oudah",
                    ResidenceCountry = "Palestine",
                    ResidenceCity = "Khanyounis",
                    ResidenceStreet = "Osama street",
                    PasswordHash = hasher.HashPassword(null, "Moh654321"),
                    UserType = UserTypes.SuperAdmin
                }
                );


            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Email = "aslgkajsldg@gmail.com",
                    IdNumber = "2354324356",
                    PhoneNumber = "8345389634",
                    UserFirstName = "Ahmed",
                    UserMiddleName = "Khaled",
                    UserLastName = "Elgendy",
                    ResidenceCountry = "Palestine",
                    ResidenceCity = "Khanyounis",
                    ResidenceStreet = "Osama street",
                    PasswordHash = hasher.HashPassword(null, "secret"),
                    UserType = UserTypes.GymAdmin,
                    GymId = DataSeederGuid
                }
                );*/


            /*modelBuilder.Entity<Gym>().HasData(
                new Gym
                {
                    Id = DataSeederGuid,
                    GymName = "Golds Gym",
                    GymCountry = "Palestine",
                    GymCity = "Gaza",
                    GymStreet = "AlRemal street"
                }
                );*/


        }

        //public DbSet<User> Users { get; set; }

    }
}
