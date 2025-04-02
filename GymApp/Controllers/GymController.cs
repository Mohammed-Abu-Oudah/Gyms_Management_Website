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
    [Route("api/[controller]")] // This way of routing is callled attribute based routing.
    [ApiController]
    public class GymController : ControllerBase
    {
        // Setting up the injections for unitOfWork and logger.
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GymController> _logger;
        private IMapper _mapper; // Before injecting the mapper, we where using the Gym module itself, now we will be using the GymDTO;
        private readonly IDataFetcher _dataFetcher;
        private readonly UserManager<User> _userManager;

        public GymController(IUnitOfWork unitOfWork, ILogger<GymController> logger, IMapper mapper, IDataFetcher dataFetcher, UserManager<User> userManager)
        {
            // Don't forget to register the unit of work in your program file.
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _dataFetcher = dataFetcher;
            _userManager = userManager;
        }

        // Remember in your request (espacially the POST) any extra information added to the object sent are ignored.

        [HttpGet("GetAllGyms")]
        public async Task<IActionResult> GetGyms()
        {
            try
            {
                var gyms = await _unitOfWork.Gyms.GetAll(); // This line will return a gym module and not a gymDTO
                // Now we will map the result to a Gym DTO by using a variable;
                var results = _mapper.Map<IList<GymDTO>>(gyms);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetGyms)}");
                return StatusCode(500, "Internal ServerError. Please try again"); // Status code 500 is universal code to say that there has been a server issue. 
            }
        }

        // Get gym by Id
        [HttpGet("{id:Guid}", Name = "GetGym")] // this name attribute tell the function that this is your name.
        public async Task<IActionResult> GetGym(Guid id)
        {
            try
            {
                var gym = await _unitOfWork.Gyms.Get(g => g.Id == id, new List<string> { "Users" });

                var result = _mapper.Map<GymDTO>(gym);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetGym)}");
                return StatusCode(500, "Internal server error. Please try again later. ");
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost(Name = "CreateNewGym")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreateGym([FromBody] CreateGymDTO gymDTO)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateGym)}");
                return BadRequest(ModelState);
            }


            try
            {
                var gym = _mapper.Map<Gym>(gymDTO);
                await _unitOfWork.Gyms.Insert(gym);
                await _unitOfWork.Save(); // See here, it's not like the get methods, here we have to execute the save function.

                return CreatedAtRoute("GetGym", new { id = gym.Id }, gym); // Used this one since it returns the created object by the client.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateGym)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }

        }

        //[HttpGet("GetCurrentLoggedInUserId")]
        //public async Task<IActionResult> GetLoggedInUserId()
        //{
        //    try
        //    {
        //        var userID = _dataFetcher.GetCurrentUserId();

        //        return Ok(userID);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal Server Error, Please Try Again Later.");
        //    }
        //}

        [Authorize(Roles = "SuperAdmin, GymAdmin")]
        [HttpPut("{id:Guid}",Name = "UpdateGym")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateGym(Guid id, [FromBody] CreateGymDTO gymDTO) // For the post method, we have two ways of thought here: 
            // 1- We can pass a GymDTO instead of CreateGymDTO, and then from this DTO we take the Id and update to the Gym with this Id the new values, the problem here is that we will violate the signle resposiblity principle that we are using.
            // 2- We can pass the Gym Id and a CreateGymDTO with the new values, and the method shall search for this Id in the database and update it's values with the new values.
            // 3- If Data you want for the Update is different than that you want for the Create, you may consider adding new DTO.
        {
            // Try in the future to add the GymAdmin role to be able to edit the gym data.
            // do this by checking the current userId and ensuring that the user is an admin for that Gym.

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(CreateGym)}");
                return BadRequest(ModelState);
            }

            var currentUserId = _dataFetcher.GetCurrentUserId();

            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            var isThisUserAGymAdmin = async () =>
            {
                var roles = await _userManager.GetRolesAsync(currentUser);
                if (roles.Contains("GymAdmin"))
                    return true;
                return false;
            };

            bool isUserGymAdmin = await isThisUserAGymAdmin();

            var gymUpdateProcedure = async () =>
            {

                try
                {
                    var gym = await _unitOfWork.Gyms.Get(g => g.Id == id);
                    if (gym == null)
                    {
                        _logger.LogError($"Invalid UPDATE attempt in {nameof(CreateGym)}");
                        return BadRequest("Gym for provided Id wasn't found");
                    }

                    _mapper.Map(gymDTO, gym); // This is just another way of using the mapping, we you put the sorce and the destination.
                    _unitOfWork.Gyms.Update(gym);
                    await _unitOfWork.Save();

                    return Ok(gymDTO);

                    //return NoContent();  // This doesn't mean a bad request it just means that Ok I done it and there's nothing that I can tell you.
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateGym)}");
                    return StatusCode(500, "Internal Server Error. Please Try Again Later.");
                }

            };

            // Check if this user is an admin for this Gym
            try
            {
                if (isUserGymAdmin)
                {
                    if (currentUser.GymId.Equals(id))
                    {
                        return await gymUpdateProcedure();
                    }
                    else
                    {
                        _logger.LogError($"User Not Authorized to perform this operation");
                        return Unauthorized("User not Authorized");
                    }
                }
                else // You might find this weird, but the logic is that if the current user is a GymAdmin => then I want to check if this user is an admin for this gym. But, if the user wasn't an admin for this gym then an error raised.
                // And finally if the user wasn't a gym admin at all then it must be a SuperAdmin according to the restrictions I added in the Aurthorize attribute.
                {
                    return await gymUpdateProcedure();
                }

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateGym)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }


        }


        [Authorize(Roles = "SuperAdmin, GymAdmin")]
        [HttpDelete("{id:Guid}", Name = "DeleteGym")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteGym(Guid id) // For the post method, we have to ways of thought here: 
                                                                                            // 1- We can pass a GymDTO instead of CreateGymDTO, and then from this DTO we take the Id and update to the Gym with this Id the new values, the problem here is that we will violate the signle resposiblity principle that we are using.
                                                                                            // 2- We can pass the Gym Id and a CreateGymDTO with the new values, and the method shall search for this Id in the database and update it's values with the new values.
                                                                                            // 3- If Data you want for the Update is different than that you want for the Create, you may consider adding new DTO.
        {
            // Try in the future to add the GymAdmin role to be able to edit the gym data.
            // do this by checking the current userId and ensuring that the user is an admin for that Gym.

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Delete attempt in {nameof(CreateGym)}");
                return BadRequest(ModelState);
            }

            var currentUserId = _dataFetcher.GetCurrentUserId();

            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            var isThisUserAGymAdmin = async () =>
            {
                var roles = await _userManager.GetRolesAsync(currentUser);
                if (roles.Contains("GymAdmin"))
                    return true;
                return false;
            };

            bool isUserGymAdmin = await isThisUserAGymAdmin();

            Func<Task<IActionResult>> gymDeleteProcedure = async () =>
            {

                try
                {
                    var gym = await _unitOfWork.Gyms.Get(g => g.Id == id);
                    if (gym == null)
                    {
                        _logger.LogError($"Invalid DELETE attempt in {nameof(CreateGym)}");
                        return BadRequest("Gym for provided Id wasn't found");
                    }


                    await _unitOfWork.Gyms.Delete(id);
                    await _unitOfWork.Save();

                    return NoContent();

                    //return NoContent();  // This doesn't mean a bad request it just means that Ok I done it and there's nothing that I can tell you.
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateGym)}");
                    return StatusCode(500, "Internal Server Error. Please Try Again Later.");
                }

            };


            // Check if this user is an admin for this Gym
            try
            {
                if (isUserGymAdmin)
                {
                    if (currentUser.GymId.Equals(id))
                    {
                        return await gymDeleteProcedure();
                    }
                    else
                    {
                        _logger.LogError($"User Not Authorized to perform this operation");
                        return Unauthorized("User not Authorized");
                    }
                }
                else // You might find this weird, but the logic is that if the current user is a GymAdmin => then I want to check if this user is an admin for this gym. But, if the user wasn't an admin for this gym then an error raised.
                // And finally if the user wasn't a gym admin at all then it must be a SuperAdmin according to the restrictions I added in the Aurthorize attribute.
                {
                    return await gymDeleteProcedure();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateGym)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }


        }


    }
}
