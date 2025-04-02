using GymApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace GymApp.Configurations.Entities
{
    public class UserTableConfigurations : IEntityTypeConfiguration<User>
    {
        // I'll only make a constructor because in the seeding of the data to provide a GymId to the user
        // This GymId which I'll declare in the DatabaseContext class.

        public UserTableConfigurations()
        {
        }
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // In seeding the data here, we won't use the builder.Entity<User> since we already told the
            // entity to take from the type user.

            // We need to get the predefined guid so we can assign the users to a specific rules.
            var guids = new GuidsSeederDefinition();

            // Properties definitions.
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasDefaultValue(Guid.NewGuid().ToString());

            builder.Property(u => u.UserName)
                .HasComputedColumnSql("[Email]");

            builder.Property(u => u.Email)
                .IsRequired(true);

            builder.Property(u => u.PasswordHash)
                .IsRequired(true);

            builder.Property(prop => prop.DisplayName)
                .HasComputedColumnSql("[UserFirstName] + ', ' + [UserMiddleName] + ', ' + [UserLastName]")
                .HasMaxLength(70)
                .IsRequired();

            builder.Property(p => p.FullAddress)
                .HasComputedColumnSql("[ResidenceCountry] + '/' + [ResidenceCity] + '/' + [ResidenceStreet]")
                .HasMaxLength(200)
                .IsRequired();

            builder.HasOne(user => user.TrainingPlan)
                .WithOne(trainingPlan => trainingPlan.User)
                .HasForeignKey<TrainingPlan>(trainingPlan => trainingPlan.UserId)
                .IsRequired();


            // Password hasher
            var hasher = new PasswordHasher<IdentityUser>();

            // For the data seeding we will only seed the SuperAdmin, which is only one and responsible for adding new admins to the database or deleting them.
           builder.HasData(
                new User
                {
                    Id = guids.SuperAdminGuid.ToString(),
                    Email = "medo.doood2211@gmail.com",
                    NormalizedEmail = "MEDO.DOOOD2211@GMAIL.COM",
                    NormalizedUserName = "MEDO.DOOOD2211@GMAIL.COM",
                    IdNumber = "406959007",
                    PhoneNumber = "0592724313",
                    UserFirstName = "Mohammed",
                    UserMiddleName = "Farid",
                    UserLastName = "Abu-Oudah",
                    ResidenceCountry = "Palestine",
                    ResidenceCity = "Khanyounis",
                    ResidenceStreet = "Osama street",
                    PasswordHash = hasher.HashPassword(null, "@Moh654321")
                }
                );


            builder.HasData(
                new User
                {
                    Id = guids.GymAdminGuid.ToString(),
                    Email = "aslgkajsldg@gmail.com",
                    NormalizedEmail = "ASLGKAJSLDG@GMAIL.COM",
                    NormalizedUserName = "ASLGKAJSLDG@GMAIL.COM",
                    IdNumber = "2354324356",
                    PhoneNumber = "8345389634",
                    UserFirstName = "Ahmed",
                    UserMiddleName = "Khaled",
                    UserLastName = "Elgendy",
                    ResidenceCountry = "Palestine",
                    ResidenceCity = "Khanyounis",
                    ResidenceStreet = "Osama street",
                    PasswordHash = hasher.HashPassword(null, "@Ahmed12345"),
                    GymId = guids.GymGuid,
                    LockoutEnabled = true
                }
                );

        }
    }
}
