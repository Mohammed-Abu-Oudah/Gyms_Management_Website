using GymApp.Data;
using GymApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GymApp.AuthServices
{
    public class AuthManager : IAuthManager
    {
        // remember to register this services inside your program.cs file as follows.
        // services.AddScoped<IAuthManager, AuthManager>();

        // we need to inject somethings. 

        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User _user;


        public AuthManager(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> CreateToken()
        {
            // we need to get somethings so we can generate the token based on it.
            var signingCredentials = GetSigningCredenials();
            var claims = await GetClaims();
            var tokenOptions = GeneratedTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private JwtSecurityToken GeneratedTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            // This one is going to combine the signing credentials and the claims and create the actual token to be issued to the user.
            var jwtSettings = _configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddMinutes(
                Convert.ToDouble(jwtSettings.GetSection("lifetime").Value));

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,  // this is the one that we specified in the service extensions for authentication.
                claims: claims,
                expires: expiration,  // This will mean that the token expires after 15 minutes of it's generation time.
                signingCredentials: signingCredentials
                );

            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {
            // we creat the claims that we want to identify the user by.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Name, _user.UserName),
            };

            // Now we need to add to the list the roles claims for this user.
            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials GetSigningCredenials()
        {
            // First we get the key and encode it
            var key = Environment.GetEnvironmentVariable("GYMAPPKEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            // Then we return the key encrypted.
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<bool> ValidateUser(LoginUserDTO loginUserDTO)
        {
            // What we want here is that we want to say does this user exist in the system and is this password valid.
            _user = await _userManager.FindByNameAsync(loginUserDTO.Email);

            return (_user != null && await _userManager.CheckPasswordAsync(_user, loginUserDTO.Password)); // this will return true or false.
        }
    }
}
