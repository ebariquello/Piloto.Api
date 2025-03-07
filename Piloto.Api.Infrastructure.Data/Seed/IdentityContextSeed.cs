using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.Data.Seed
{
    public class IdentityContextSeed
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            using IServiceScope serviceScope = serviceProvider.CreateScope();
            var loggerFactory = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            try
            {
               
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<StockManagementIdentityDBContext>();

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
           
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<IdentityContextSeed>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }
        public static async Task SeedSuperAdminAsync(IServiceProvider serviceProvider)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "eduardo.bariquello@piloto.com",
                FirstName = "Eduardo",
                LastName = "Bariquello",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            using IServiceScope serviceScope = serviceProvider.CreateScope();
            var loggerFactory = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            try
            {

                var dbContext = serviceScope.ServiceProvider.GetRequiredService<StockManagementIdentityDBContext>();

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (userManager.Users.All(u => u.Id != defaultUser.Id))
                {
                    var user = await userManager.FindByEmailAsync(defaultUser.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(defaultUser, "pass@word1");
                        await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                        await userManager.AddToRoleAsync(defaultUser, Roles.Moderator.ToString());
                        await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                        await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<IdentityContextSeed>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }
    }
}
