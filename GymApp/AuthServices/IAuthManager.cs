using GymApp.Models;

namespace GymApp.AuthServices
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDTO loginUserDTO);

        Task<string> CreateToken();

        // Now create the class to consume these functions.
    }
}
