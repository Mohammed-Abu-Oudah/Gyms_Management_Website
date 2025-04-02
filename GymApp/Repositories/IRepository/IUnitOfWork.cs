using GymApp.Data;

namespace GymApp.Repositories.IRepository
{
    public interface IUnitOfWork : IDisposable  // its going to act like a register for every variation of the generic reposritry relative to the class T
    {
        IGenericRepository<User> Users { get; }

        IGenericRepository<Gym> Gyms { get; }

        IGenericRepository<TrainingPlan> TrainingPlans { get; }
        
        Task Save(); // When ever we made changes to the database before this function, these changes were just staged, we need to save them, we used before to use a save function. But, now we have this function inside the unit of work to commit these changes.
        // After you have created this class, go and creat a Concrete class for it.
    }
}
