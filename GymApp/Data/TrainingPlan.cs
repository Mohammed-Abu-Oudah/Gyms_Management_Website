using System.ComponentModel.DataAnnotations.Schema;

namespace GymApp.Data
{
    public class TrainingPlan
    {
        
        public Guid PlanId { get; set; }
        public int CaloriesToConsume { get; set; }
        public int NumOfTrainingHours { get; set; }
        public int NumOfTrainingDaysPerWeek { get; set; }

        //[ForeignKey(nameof(User))]
        public string UserId { get; set; } // changed the Id to string, since in the database and the identity the user id is considered as a string.
        public User User { get; set; }
        
    }
}
