
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Piloto.Api.Infrastructure.Data.Migrations
{
    public class DBManagementService
    {
        public static void MigrationInitialization(IServiceProvider serviceProvider)
        {
            using IServiceScope? serviceScope = serviceProvider.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<StockManagementDBContext>();
            if (!dbContext.Database.CanConnect())
            {
                // If not, create the database
                dbContext.Database.EnsureCreated();
            }
            try { 
            serviceScope.ServiceProvider.GetService<StockManagementDBContext>()?.Database.Migrate();
            }
            catch (Exception ex) { }
        }
        public static void MigrationIdentityInitialization(IServiceProvider serviceProvider)
        {
            using IServiceScope? serviceScope = serviceProvider.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<StockManagementIdentityDBContext>();
            if (!dbContext.Database.CanConnect())
            {
                // If not, create the database
                dbContext.Database.EnsureCreated();
            }
            try
            {
                serviceScope.ServiceProvider.GetService<StockManagementIdentityDBContext>()?.Database.Migrate();
            }
            catch (Exception ex) { }
        }
    }
}
