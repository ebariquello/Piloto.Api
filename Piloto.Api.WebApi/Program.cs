using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Piloto.Api.Domain.Models;
using Piloto.Api.Infrastructure.Data;
using Piloto.Api.Infrastructure.Data.Repository;
using System.Text;

using Microsoft.AspNetCore;
using Piloto.Api.Infrastructure.Data.Seed;
using Piloto.Api.WebApi;


// Add services to the container.
// Database Context
//var connStr = builder.Configuration.GetConnectionString("MySQLHostMachineConn");

//builder.Services.ConfigureServices(builder.Configuration);

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = false;
//    options.User.RequireUniqueEmail = false;
//    //options.User.AllowedUserNameCharacters = true;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireDigit = false;
//})
//.AddRoles<IdentityRole>()
//.AddEntityFrameworkStores<MySQLStockManagementIdentityDBContext>()
//.AddDefaultTokenProviders();


//// JWT Configuration
//var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidIssuer = jwtSettings["Issuer"],
//        ValidAudience = jwtSettings["Audience"],
//        ClockSkew = TimeSpan.Zero
//    };
//});

//builder.Services.AddAuthorization();

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
namespace Piloto.Api.WebApi;
public class Program
{
    public async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await IdentityContextSeed.SeedRolesAsync(services);
                await IdentityContextSeed.SeedSuperAdminAsync(services);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }
        host.Run();
    }

    public static IWebHostBuilder CreateHostBuilder(string[] args) =>
         WebHost.CreateDefaultBuilder(args)
             .ConfigureLogging((hostingContext, logging) =>
             {
                 logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                 logging.AddConsole();
                 logging.AddDebug();
             })
             .UseStartup<Startup>();
    //.Build();
}
