using GymApp.ContextsAndFlunetAPIs;
using GymApp.Credentials;
using GymApp.Data;
using GymApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace GymApp.Services
{
    // Now in this class, we will configure the services instead of keep adding to the program class
    public static class ServiceExtensions
    {

        public static void ConfigureIdentity(this IServiceCollection services)  // This keyword means that this is an extension method, which makes us able to provide extra functionality to void methods
        {
            var builder = services.AddIdentityCore<User>(q =>
            {
                q.User.RequireUniqueEmail = true;
                q.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
                q.SignIn.RequireConfirmedAccount = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);

            builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

        }

        // Now we need to configure the JWT services.
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration) 
        {
            var jwtSettings = configuration.GetSection("Jwt"); // this is the name that you specified in appsettings file.
            var key = Environment.GetEnvironmentVariable("GYMAPPKEY"); // this is the name that you put we setting the environemnt variables.


            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // what this means is that, whenever someone tries to authenticate, then check for a bearer token 
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Whatever information comes across, challenge it based on the jwt.
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateAudience = false
                };
            });

        }

    }
}
