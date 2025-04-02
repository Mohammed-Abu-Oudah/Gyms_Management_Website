

using FluentValidation;
using GymApp.AuthServices;
using GymApp.BusinessLogic.DataLogic;
using GymApp.BusinessLogic.IDataLogic;
using GymApp.Configurations;
using GymApp.ContextsAndFlunetAPIs;
using GymApp.Data;
using GymApp.Models;
using GymApp.Repositories.IRepository;
using GymApp.Repositories.Repository;
using GymApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(option =>
    option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

AddSwaggerDoc(builder.Services);

void AddSwaggerDoc(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Aurthorization header using the Bearer scheme.
                Enter 'Bearer' [space] and then your token in the text input below.
                Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "0auth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
            new List<string>()
            }
        });
    });
}

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectionString")) // This configuration is programmed to get the configurations from the appsettings.json file, so it will understand that it should get the sql connection from the appsettings file.
);


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddCors(option =>
{
    option.AddPolicy(name: "CorsPolicy", configurePolicy: builder =>
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});

// Here we will add the automapper that we created.
builder.Services.AddAutoMapper(typeof(MapperInitializer));


// After we have made the flunet api validation inside the User file and made the condition
// We can register the validator either automatically as we are going to do here, or manually as in this link:
// https://docs.fluentvalidation.net/en/latest/aspnet.html
//https://docs.fluentvalidation.net/en/latest/conditions.html

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IValidator<CreateUserDTO>, CreateUserDTOValidator>();

// We need to register the controllers we created.
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IDataFetcher, DataFetcher>();

builder.Host.UseSerilog();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllers();


app.UseCors("CorsPolicy"); //inside we adde the name of the policy we defined when creating the policy.


// Here we configure a logger object so we can add it to the configurations in serilog.

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(path: "D:\\Projects\\GymApp\\Logs\\log-.txt",
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
    rollingInterval: RollingInterval.Day, // This rolling interval means that everyday the program is going to generate a log file with the data associated with the name of the file and thats why we added (-) at the end of the file name to split the date from the name
    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose // This configuration is used to express the amount of details need to added inside the log
    ).CreateLogger(); // without this infromation, so many errors will popup, this function prompts the code to creat a logger object.

try
{
    Log.Information("Application Is Starting");
    app.Run(); // We added the app runner here, since if any exception happened when running the app, then we can catch it.

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");

}
finally
{
    Log.CloseAndFlush();
}

