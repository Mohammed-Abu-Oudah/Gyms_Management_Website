using AutoMapper;
using GymApp.BusinessLogic.DataLogic;
using GymApp.Data;
using GymApp.Models;
using GymApp.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace GymApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public IUnitOfWork _unitOfWork;

        public ILogger<UsersController> _logger;

        public IMapper _mapper;

        private readonly UserManager<User> _userManager; // Added this line in order to be able to get roles of users.

        private readonly IDataFetcher _dataFetcher;

        public UsersController(IUnitOfWork unitOfWork, ILogger<UsersController> logger, IMapper mapper, UserManager<User> userManager, IDataFetcher dataFetcher)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _dataFetcher = dataFetcher;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAll();
                var result = _mapper.Map<IList<UserDTO>>(users);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Something went wrong in the {nameof(GetUsers)}.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                var user = await _unitOfWork.Users.Get(u => u.Id.Equals(id), new List<string> { "Gym" });
                var result = _mapper.Map<UserDTO>(user);
                
                // This part is optional in case you want to included the rules of the user in the response.

                var roles = await _userManager.GetRolesAsync(user);

                result.Roles = roles;

                return Ok(result);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, $"Something went wrong in {nameof(GetUser)}");
                return StatusCode(500, "Internal server error. Please try again later");
            }
        }

        [Authorize(Roles = "SuperAdmin, GymAdmin, Trainee")]
        [HttpPut("{id:Guid}", Name="UpdateUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdateUser(Guid id, [FromBody]UpdateUserDTO updateUserDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateUser)}");
                return BadRequest(ModelState);
            }
            // The logic here goes as follows: 
            // 1- Get the current user id
            // 2- If the current user is a gym admin: Ensure that his gymId is the same as the gym admin id to ensure that this is the admin of his gym.
            // 3- If the current user is a trainee, so he wants to update his account, this means that the given Id for update should be the same as user id.

            var currentUserId = _dataFetcher.GetCurrentUserId();

            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            var userToBeUpdated = await _userManager.FindByIdAsync(id.ToString());

            var returnCurrentUserRoles = async () =>
            {
                var roles = await _userManager.GetRolesAsync(currentUser);
                return roles;
            };

            Func<Task<IActionResult>> userUpdateProcedure = async () =>
            {

                try
                {
                    if (userToBeUpdated == null)
                    {
                        _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateUser)}");
                        return BadRequest("User of the provided id doesn't exist");
                    }

                    _mapper.Map(updateUserDTO, userToBeUpdated);

                    var updateResult = await _userManager.UpdateAsync(userToBeUpdated);

                    if (updateResult.Succeeded)
                    {
                        return Ok(updateUserDTO);
                    }

                    return BadRequest("Sorry, the user wasn't updated successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateUser)}");
                    return StatusCode(500, "Internal Server Error. Please Try Again Later.");
                }

            };

            try
            {
                var currentUserRoles = await returnCurrentUserRoles();

                if (currentUserRoles.Contains("SuperAdmin"))
                {
                    return await userUpdateProcedure();
                }
                else if (currentUserRoles.Contains("GymAdmin"))
                {
                    if (currentUser.GymId.Equals(userToBeUpdated.GymId))
                        return await userUpdateProcedure();
                    else
                        return Unauthorized("User Not Authorized");
                }
                else if (currentUserRoles.Contains("Trainee"))
                {
                    if (currentUser.Id.Equals(userToBeUpdated.Id))
                        return await userUpdateProcedure();
                    else
                        return Unauthorized("User Not Authorized");
                }
                else
                {
                    return Unauthorized("User Not Authorized");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateUser)}.");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }

        }

        [Authorize]
        [HttpDelete("{userId:Guid}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Delete Attempt in {nameof(DeleteUser)}.");
                return BadRequest(ModelState);
            }

            var userToBeDeleted = await _userManager.FindByIdAsync(userId.ToString());

            if (userToBeDeleted == null)
            {
                return BadRequest($"The User with the provided Id doesn't exist.");
            }

            var currentUserId = _dataFetcher.GetCurrentUserId();

            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);


            // What's going to happen is as follows
            // 1- If this is the User who want to delete his account, delete whether it's a tainer or a trainee.
            // 2- If the user is a gym admin, then ensure that this is the gym admin for the current user.
            // 3- If the user is a Super admin then delete.

            var canTheUserDelete = () =>
            {
                if (currentUserRoles.Contains("SuperAdmin"))
                {
                    return true;
                }
                else if (currentUserRoles.Contains("GymAdmin"))
                {
                    // Ensure that this is the gymAdmin for the user.
                    // First let's get the gym for the User.

                    var userToDeleteGymId = userToBeDeleted.GymId;
                    var currentUserGymId = currentUser.GymId;

                    if(userToDeleteGymId.Equals(currentUserGymId))
                    {
                        return true;
                    }
                }
                
                // If the User isn't a SuperAdmin or a GymAdmin.

                if(currentUserId.Equals(userId.ToString()))
                {
                    return true;
                }

                return false;
            };

            try
            {
                if (canTheUserDelete())
                {
                    var deleteResult = await _userManager.DeleteAsync(userToBeDeleted);
                    if (deleteResult.Succeeded)
                    {
                        _logger.LogInformation($"User has been deleted successfully in the {nameof(DeleteUser)}.");
                        return NoContent();
                    }
                    else
                    {
                        _logger.LogError($"Something wen't wrong in the {nameof(DeleteUser)}, User delete operation didn't succeed");
                        return BadRequest();
                    }
                }
                else
                {
                    _logger.LogError($"User Not Authorized.");
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteUser)}.");
                return StatusCode(500, $"Internal server error, please try again later.");
            }


        }

    }
}
