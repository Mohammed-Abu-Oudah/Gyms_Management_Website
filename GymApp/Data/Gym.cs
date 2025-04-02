namespace GymApp.Data
{
    public class Gym
    {
        public Guid Id { get; set; }
        public string GymName { get; set; }
        public string GymCountry { get; set; }
        public string GymCity { get; set; }
        public string GymStreet { get; set; }

        public List<User>? Users { get; set; }
    }
}
