using Microsoft.AspNetCore.Identity;

namespace GymApp.Credentials
{
    public class ApiUser : IdentityUser    // You can use the fields of the identity user no problem, but if you need to extend them and user more fields, you can make a class that implements the Identity user and provide it to the IdentityDbContext.
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
