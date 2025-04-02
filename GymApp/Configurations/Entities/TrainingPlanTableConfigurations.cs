using GymApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace GymApp.Configurations.Entities
{
    public class TrainingPlanTableConfigurations : IEntityTypeConfiguration<TrainingPlan>
    {

        public void Configure(EntityTypeBuilder<TrainingPlan> builder)
        {
            builder.HasKey(plan => plan.PlanId);
        }

    }
}
