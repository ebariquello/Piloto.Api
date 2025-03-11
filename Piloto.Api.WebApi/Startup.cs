//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using Piloto.Api.Application;
//using Piloto.Api.Domain.Models;
//using Piloto.Api.Domain.Services;
//using Piloto.Api.Infrastructure.CrossCutting.Adapter;
//using Piloto.Api.Infrastructure.Data.Migrations;
//using Piloto.Api.Infrastructure.Data.Repository;
//using Piloto.Api.Infrastructure.Data;
//using System.Text;
//using Swashbuckle.AspNetCore.SwaggerUI;
//using Microsoft.AspNetCore.Builder;

//public class Startup
//{
//    public Startup(IConfiguration configuration)
//    {
//        Configuration = configuration;
//    }

//    public IConfiguration Configuration { get; }

//    // This method gets called by the runtime. Use this method to add services to the container.
//    public void ConfigureServices(IServiceCollection services)
//    {
//        services.AddDataServices(Configuration);
//        services.AddCrossCuttingAdapterServicesDI();
//        services.AddRepositoryServicesDI();
//        services.AddServicesDI();
//        services.AddApplicationServicesDI();

//        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
//        {
//            options.SignIn.RequireConfirmedAccount = false;
//            options.User.RequireUniqueEmail = false;
//            options.Password.RequireLowercase = false;
//            options.Password.RequireUppercase = false;
//            options.Password.RequireDigit = false;
//        })
//        .AddRoles<IdentityRole>()
//        .AddEntityFrameworkStores<StockManagementIdentityDBContext>()
//        .AddDefaultTokenProviders();

//        // Configure Authentication with JWT
//        var jwtSettings = Configuration.GetSection("JwtSettings");
//        var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

//        services.AddAuthentication(options =>
//        {
//            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//        })
//        .AddJwtBearer(options =>
//        {
//            options.RequireHttpsMetadata = false;
//            options.SaveToken = true;
//            options.TokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = new SymmetricSecurityKey(key),
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidIssuer = jwtSettings["Issuer"],
//                ValidAudience = jwtSettings["Audience"],
//                ClockSkew = TimeSpan.Zero
//            };
//            options.Events = new JwtBearerEvents
//            {
//                OnAuthenticationFailed = context =>
//                {
//                    context.Response.Headers.Add("Authentication-Failed", context.Exception.Message);
//                    return Task.CompletedTask;
//                },
//                OnTokenValidated = context =>
//                {
//                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
//                    logger.LogInformation("Token validated successfully.");
//                    return Task.CompletedTask;
//                },
//                OnChallenge = context =>
//                {
//                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
//                    logger.LogWarning("OnChallenge error: {Error}", context.Error);
//                    logger.LogWarning("OnChallenge error description: {ErrorDescription}", context.ErrorDescription);
//                    logger.LogWarning("OnChallenge request path: {Path}", context.Request.Path);
//                    logger.LogWarning("OnChallenge request headers: {Headers}", context.Request.Headers);
//                    return Task.CompletedTask;
//                }
//            };
//        });

//        services.AddAuthorization();

//        // Add Controllers
//        services.AddControllers();

//        // Configure Swagger with JWT Support
//        services.AddSwaggerGen(c =>
//        {
//            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

//            var securityScheme = new OpenApiSecurityScheme
//            {
//                Name = "Authorization",
//                Description = "Enter 'Bearer {token}'",
//                In = ParameterLocation.Header,
//                Type = SecuritySchemeType.Http,
//                Scheme = "bearer",
//                BearerFormat = "JWT"
//            };

//            c.AddSecurityDefinition("Bearer", securityScheme);
//            c.AddSecurityRequirement(new OpenApiSecurityRequirement
//            {
//                { securityScheme, new string[] {} }
//            });
//        });
//    }

//    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//    {
//        // Configure the HTTP request pipeline.
//        if (env.IsDevelopment() || env.EnvironmentName.Contains("Docker") || env.EnvironmentName.Contains("DevelopmentLocal"))
//        {
//            var jwtSettings = Configuration.GetSection("JwtSettings");
//            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

//            app.UseDeveloperExceptionPage();
//            app.UseSwagger();
//            app.UseSwaggerUI(c =>
//            {
//                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
//                c.DocumentTitle = "My API Documentation";
//                c.DocExpansion(DocExpansion.None);
//                c.DefaultModelsExpandDepth(-1); // Hide models section
//                c.OAuthClientId("your-client-id");
//                c.OAuthClientSecret(jwtSettings["Secret"]);
//                c.OAuthRealm("your-realm");
//                c.OAuthAppName("My API v1");
//                c.OAuthScopeSeparator(" ");
//                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
//            });
//            DBManagementService.MigrationInitialization(app.ApplicationServices);
//            DBManagementService.MigrationIdentityInitialization(app.ApplicationServices);
//        }
//        else
//        {
//            app.UseExceptionHandler("/Error");
//            app.UseHsts();
//        }

//        app.UseHttpsRedirection();

//        app.UseRouting();

//        app.UseAuthentication();
//        app.UseAuthorization();

//        app.UseEndpoints(endpoints =>
//        {
//            endpoints.MapControllers();
//        });
//    }
//}
