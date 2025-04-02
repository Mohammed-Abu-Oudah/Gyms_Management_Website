using GymApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Emit;

namespace GymApp.Configurations.Entities
{
    public class GymTableConfigurations : IEntityTypeConfiguration<Gym>
    {

        public GymTableConfigurations()
        {
        }
        public void Configure(EntityTypeBuilder<Gym> builder)
        {
            var guids = new GuidsSeederDefinition();

            builder.HasKey(gym => gym.Id);

            builder.HasMany(gym => gym.Users)
                .WithOne(user => user.Gym)
                .HasForeignKey(user => user.GymId)
            .IsRequired(false);

            builder.Property(gym => gym.GymName)
                .IsRequired();


            builder.HasData(
                new Gym
                {
                    Id = guids.GymGuid,
                    GymName = "Golds Gym",
                    GymCountry = "Palestine",
                    GymCity = "Gaza",
                    GymStreet = "AlRemal street"
                }
                );
        }
    }
}
