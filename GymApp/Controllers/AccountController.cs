using AutoMapper;
using GymApp.AuthServices;
using GymApp.Credentials;
using GymApp.Data;
using GymApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


// this is the class the will be controlling the user regestration.
namespace GymApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        /*private readonly SignInManager<User> _signInManager;*/ // we don't need the SignInManager since it's job will be replaced by tokens.
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountController(UserManager<User> userManager, ILogger<AccountController> logger, IMapper mapper, IAuthManager authManager)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        // after we satup the controller and it's constructor, we're going to setup the endpoints.

        // First endpoint is going to be the regestration.
        [HttpPost]  // Used post since I don't want to put the registration data across the pipeline and it will be visible in a plain sites.
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO createUserDTO)
        {

            //var hasher = new PasswordHasher<IdentityUser>();  this is used in case we wanted to hash the password ourselves which will delete the restrictions of the password implied by asp.net and we can only use our customized restrictions.

            _logger.LogInformation($"Registration attempt for {createUserDTO.Email}");
            if (!ModelState.IsValid) // This is just something like a from validation could be more clear when usign MVC.
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<User>(createUserDTO);
                user.Email = createUserDTO.Email;
                user.UserName = createUserDTO.Email;
                //user.PasswordHash = hasher.HashPassword(null, createUserDTO.Password);
                var result = await _userManager.CreateAsync(user, createUserDTO.Password);

                if(!result.Succeeded)
                {

                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return BadRequest(ModelState);
                }
                // After things go fine, we will add the user to the Roles and assign the roles to it.
                await _userManager.AddToRolesAsync(user, createUserDTO.Roles);

                return Accepted(createUserDTO);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500); // Just a different way instead of using directly status code 500.
            }

        }


        [HttpPost]  // Now for the login function
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO) // Using this login user DTO is for extra security in case the user sent malicious data in the request, I'll only be looking for specific parts.
        {

            _logger.LogInformation($"Login attempt for {loginUserDTO.Email}");
            if (!ModelState.IsValid) // This is just something like a from validation could be more clear when usign MVC.
            {
                return BadRequest(ModelState);
            }

            try
            {

                if (!await _authManager.ValidateUser(loginUserDTO))
                {
                    return Unauthorized();
                }

                return Accepted(new {Token = await _authManager.CreateToken()});
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500); // Just a different way instead of using directly status code 500.
            }

        }


    }
}
