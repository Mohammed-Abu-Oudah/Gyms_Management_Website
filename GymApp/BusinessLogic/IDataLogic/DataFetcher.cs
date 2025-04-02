using GymApp.BusinessLogic.DataLogic;
using GymApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymApp.BusinessLogic.IDataLogic
{
    public class DataFetcher : IDataFetcher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DataFetcher> _logger;
        
        public DataFetcher(IHttpContextAccessor httpContextAccessor, ILogger<DataFetcher> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public string GetCurrentUserId()
        {
            _logger.LogInformation("Attempt to fetch user id");

            try
            {
                ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;

                Claim userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null && !string.IsNullOrEmpty(userIdClaim.Value))
                {
                    return userIdClaim.Value;
                }
                else
                {
                    _logger.LogError($"Couldn't fetch claim id for {nameof(user)}");
                    throw new InvalidOperationException("User Claim not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCurrentUserId)}. User Id could't be fetched.");
                return null;
            }
            
        }
    }
}
