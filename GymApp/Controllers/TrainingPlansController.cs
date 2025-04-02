
using AutoMapper;
using GymApp.BusinessLogic.DataLogic;
using GymApp.Data;
using GymApp.Models;
using GymApp.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingPlansController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TrainingPlansController> _logger;
        private IMapper _mapper;
        private readonly IDataFetcher _dataFetcher;
        private readonly UserManager<User> _userManager;

        public TrainingPlansController(IUnitOfWork unitOfWork, ILogger<TrainingPlansController> logger, IMapper mapper, IDataFetcher dataFetcher, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _dataFetcher = dataFetcher;
            _userManager = userManager;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("GetAllTrainingPlans")]
        public async Task<IActionResult> GetTrainingPlans()
        {
            try
            {
                var traniningPlans = await _unitOfWork.TrainingPlans.GetAll();

                var results = _mapper.Map<IList<TrainingPlanDTO>>(traniningPlans);

                _logger.LogInformation("Successful Get All training plans attempt");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Something went wrong in the {nameof(GetTrainingPlans)}.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet("{trainingPlanId:Guid}", Name = "GetTrainingPlan")]
        public async Task<IActionResult> GetTrainingPlan(Guid trainingPlanId)
        {

            var trainingPlan = await _unitOfWork.TrainingPlans.Get(p => p.PlanId.Equals(trainingPlanId));

            if (trainingPlan == null)
            {
                _logger.LogError($"Failed training plan get attempt in {nameof(GetTrainingPlan)}");
                return BadRequest("No training plans exist with the requested id");
            }

            var result = _mapper.Map<GetTrainingPlanDTO>(trainingPlan);

            var currentUserId = _dataFetcher.GetCurrentUserId();

            var currentUser = await _userManager.FindByIdAsync(currentUserId);


            var returnCurrentUserRoles = async () =>
            {
                var roles = await _userManager.GetRolesAsync(currentUser);
                return roles;
            };

            var canUserAccessTrainingPlan = async (string traniningPlanUserID) =>
            {
                var currentUserRoles = await returnCurrentUserRoles();

                var currentUserGymId = currentUser.GymId;

                var trainingPlanUser = await _userManager.FindByIdAsync(traniningPlanUserID.ToString());

                var trainingPlanUserGymId = trainingPlanUser.GymId;

                if (currentUserRoles.Contains("SuperAdmin"))
                {
                    return true;
                }
                else if (currentUserId.Equals(traniningPlanUserID))
                {
                    return true;
                }
                else if (currentUserRoles.Contains("GymAdmin") || currentUserRoles.Contains("Trainer"))
                {
                    // Now if the user is neither a super admin nor has the same Id of the user training plan we intend.
                    // Then it either has to be the gym admin of this user of a trainee in this gym.

                    if (currentUserGymId.Equals(trainingPlanUserGymId))
                    {
                        return true;
                    }

                }

                return false;
            };

            try
            {
                bool userAccessbility = await canUserAccessTrainingPlan(trainingPlan.UserId);

                if (userAccessbility)
                {
                    _logger.LogInformation($"Successful training plan access attempt in {nameof(GetTrainingPlan)}.");
                    return Ok(result);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somthing went wrong in {nameof(GetTrainingPlan)}.");

                return StatusCode(500, "Internal Server Error. Please try again later");
            }


        }

        [Authorize(Roles = "SuperAdmin, GymAdmin, Trainer")]
        [HttpPost("{userId:Guid}", Name = "CreateUserTrainingPlan")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreateUserTrainingPlan(Guid userId, [FromBody] GetTrainingPlanDTO createTrainingPlanDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateUserTrainingPlan)}.");
                return BadRequest(ModelState);
            }

            var currentUserId = _dataFetcher.GetCurrentUserId();

            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            var userToHaveTrainingPlan = await _userManager.FindByIdAsync(userId.ToString());

            // First we need to check if the user exists
            var doesUserExist = () =>
            {
                if(userToHaveTrainingPlan != null)
                {
                    return true;
                }
                return false;
            };

            bool userExistence = doesUserExist();

            var canUserAddTrainingPlan = () =>
            {
                if (!userExistence)
                    return false;

                if (currentUserRoles.Contains("SuperAdmin"))
                    return true;

                if (currentUser.GymId.Equals(userToHaveTrainingPlan.GymId))
                    return true;

                return false;
            };

            try
            {
                if(canUserAddTrainingPlan())
                {
                    var trainingPlan = _mapper.Map<TrainingPlan>(createTrainingPlanDTO);
                    trainingPlan.UserId = userId.ToString();

                    await _unitOfWork.TrainingPlans.Insert(trainingPlan);

                    await _unitOfWork.Save();

                    _logger.LogInformation($"Successfully Created training plan in {nameof(CreateUserTrainingPlan)}.");

                    return CreatedAtRoute("GetTrainingPlan", new { id = trainingPlan.PlanId }, trainingPlan);
                }

                return Unauthorized($"User Not Allowed to perform this action");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(CreateUserTrainingPlan)}.");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }

        }

        [Authorize(Roles = "SuperAdmin, GymAdmin, Trainer")]
        [HttpDelete("{trainingPlanId:Guid}", Name = "DeleteTrainingPlan")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTrainingPlan(Guid trainingPlanId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteTrainingPlan)}.");
                return BadRequest();
            }

            var trainingPlanToBeDeleted = await _unitOfWork.TrainingPlans.Get(p => p.PlanId.Equals(trainingPlanId));

            if (trainingPlanToBeDeleted == null)
            {
                _logger.LogError($"Provided training plan id doesn't exist.");
                return NotFound();
            }

            var currentUserId = _dataFetcher.GetCurrentUserId();
            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            var currentUserGymId = currentUser.GymId;

            var trainingPlanToBeDeletedGymId = async () =>
            {
                var trainingPlanToBeDeletedUserId = trainingPlanToBeDeleted.UserId;
                var trainingPlanToBeDeletedUser = await _userManager.FindByIdAsync(trainingPlanToBeDeletedUserId);
                return trainingPlanToBeDeletedUser.GymId;
            };

            var userAllowedToDelete = async () =>
            {
                var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

                var roles = currentUserRoles.ToList();

                if (currentUserRoles.Contains("SuperAdmin"))
                {
                    return true;
                }

                if (currentUserGymId.Equals(await trainingPlanToBeDeletedGymId()))
                {
                    return true;
                }

                return false;
            };

            try
            {
                if (await userAllowedToDelete())// await userAllowedToDelete()
                {
                    var operationResult = _unitOfWork.TrainingPlans.Delete(trainingPlanId);
                    await _unitOfWork.Save();

                    if (operationResult.IsCompletedSuccessfully)
                    {
                        _logger.LogInformation($"Successful delete operation in {nameof(DeleteTrainingPlan)}.");
                        return NoContent();
                    }
                    else
                    {
                        _logger.LogError($"Failed to perform delete operation for {nameof(DeleteTrainingPlan)}.");
                        return BadRequest();
                    }

                }
                else
                {
                    _logger.LogError($"Current user isn't allowed to perform the deleted action for the intended training plan.");
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(DeleteTrainingPlan)}. Please try again later.");
                return StatusCode(500, "Internal server error. Please try again later");
            }
        }

        [Authorize(Roles = "SuperAdmin, GymAdmin, Trainer")]
        [HttpPut("{id:Guid}", Name = "EditTrainingPlan")]
        [Route("EditTrainingPlan")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditTrainingPlan(Guid id, [FromBody]CreateTrainingPlanDTO newTrainingPlan)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid update attempt in {nameof(EditTrainingPlan)}");
                return BadRequest();
            }

            var trainingPlan = await _unitOfWork.TrainingPlans.Get(tp => tp.PlanId.Equals(id));

            if (trainingPlan == null)
            {
                _logger.LogError($"Training plan not found for the given ID");
                return NotFound();
            }

            var currentUserId = _dataFetcher.GetCurrentUserId();
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            var currentUserGymId = currentUser.GymId;
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            var userToHaveHisTrainingPlanChangedId = trainingPlan.UserId;
            var userToHaveHisTrainingPlanChanged = await _userManager.FindByIdAsync(userToHaveHisTrainingPlanChangedId);
            var userToHaveHisTrainingPlanChangedGymId = userToHaveHisTrainingPlanChanged.GymId;


            var userAllowedToEdit = () =>
            {
                if (currentUserRoles.Contains("SuperAdmin"))
                {
                    return true;
                }

                return currentUserGymId.Equals(userToHaveHisTrainingPlanChangedGymId); // Will either return true or false.
            };

            try
            {

                if (userAllowedToEdit())
                {
                    _mapper.Map(newTrainingPlan, trainingPlan);
                    _unitOfWork.TrainingPlans.Update(trainingPlan);
                    await _unitOfWork.Save();

                    _logger.LogError($"Training Plan has been updated successfully!");
                    return Ok(newTrainingPlan);
                }

                _logger.LogError($"User was prevented to perform action in {nameof(EditTrainingPlan)}");
                return Unauthorized($"User Not allowed to perform this action");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(EditTrainingPlan)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }

        }


    }
}
