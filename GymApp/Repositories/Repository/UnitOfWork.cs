using GymApp.ContextsAndFlunetAPIs;
using GymApp.Data;
using GymApp.Repositories.IRepository;

namespace GymApp.Repositories.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databaseContext; // This is the variable responsible for the connection to the database, and through it we would be able to commit the procedures that we have made.

        private IGenericRepository<User> _users;
        private IGenericRepository<Gym> _gyms;
        private IGenericRepository<TrainingPlan> _trainingPlans;

        public UnitOfWork(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IGenericRepository<User> Users => _users ??= new GenericRepository<User>(_databaseContext);
        
        public IGenericRepository<Gym> Gyms => _gyms ??= new GenericRepository<Gym>(_databaseContext);

        public IGenericRepository<TrainingPlan> TrainingPlans => _trainingPlans ??= new GenericRepository<TrainingPlan>(_databaseContext);

        public void Dispose()
        {
            _databaseContext.Dispose();
            GC.SuppressFinalize(this); // this dispose function is just like a garbage collector, it tells that clean up the memory after the operations are finished.
        }

        public async Task Save()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
