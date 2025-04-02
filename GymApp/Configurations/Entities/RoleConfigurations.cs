using GymApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymApp.Configurations.Entities
{
    public class RoleConfigurations : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            var guids = new GuidsSeederDefinition();
            builder.HasData(
                new IdentityRole
                {
                    Id = guids.SupeAdminRuleGuid.ToString(),
                    Name = UserTypes.SuperAdmin.ToString(),
                    NormalizedName = UserTypes.SuperAdmin.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = guids.GymAdminRuleGuid.ToString(),
                    Name = UserTypes.GymAdmin.ToString(),
                    NormalizedName = UserTypes.GymAdmin.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = guids.TrainerRuleGuid.ToString(),
                    Name = UserTypes.Trainer.ToString(),
                    NormalizedName = UserTypes.Trainer.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = guids.TraineeRuleGuid.ToString(),
                    Name = UserTypes.Trainee.ToString(),
                    NormalizedName = UserTypes.Trainee.ToString().ToUpper()
                }
                );
        }
    }
}
