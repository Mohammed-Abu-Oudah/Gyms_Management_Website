using System.ComponentModel.DataAnnotations;

namespace GymApp.Models
{
    public class GymDTO
    {
        public Guid Id { get; set; }

        public string GymName { get; set; }
        public string GymCountry { get; set; }
        public string GymCity { get; set; }
        public string GymStreet { get; set; }
        
        public List<UserDTO>? Users { get; set; }
    }

    public class CreateGymDTO
    {
        [Required]
        public string GymName { get; set; }
        [Required]
        public string GymCountry { get; set; }
        [Required]
        public string GymCity { get; set; }
        [Required]
        public string GymStreet { get; set; }
    }
}
