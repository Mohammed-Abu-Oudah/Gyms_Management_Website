namespace GymApp.Models
{

    public class GetTrainingPlanDTO
    {
        public int CaloriesToConsume { get; set; }
        public int NumOfTrainingHours { get; set; }
        public int NumOfTrainingDaysPerWeek { get; set; }
    }

    public class CreateTrainingPlanDTO : GetTrainingPlanDTO
    {
        // it's going to include the rest of properties by inheritance.
        public Guid UserId { get; set; }
    }

    public class TrainingPlanDTO : CreateTrainingPlanDTO
    {
        // it's going to include the rest of properties by inheritance.
        public Guid PlanId { get; set; }
    }


}
