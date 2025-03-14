using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.Data
{
    public static class DataServicesDI
    {
        public static void AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            var dbRunAs = configuration["DbRunAs"];
            var connStr = configuration.GetConnectionString("DefaultConnectionString");

            if(dbRunAs == "InMemory")
            {
                services.AddDbContext<StockManagementDBContext>(o =>
                o.UseInMemoryDatabase(databaseName: "StockManagementDB"), ServiceLifetime.Scoped);

                services.AddDbContext<StockManagementIdentityDBContext>(o =>
                o.UseInMemoryDatabase(databaseName: "StockManagementDB"), ServiceLifetime.Scoped);

            }
            else
            {
                services.AddDbContext<StockManagementDBContext>(o =>
                    o.UseSqlServer(connStr), ServiceLifetime.Scoped);

                services.AddDbContext<StockManagementIdentityDBContext>(o =>
                     o.UseSqlServer(connStr), ServiceLifetime.Scoped);
            }


            services.AddScoped(Func<DbContext>
             (provider) =>
                 () =>
                 {
                     return provider.GetService<StockManagementDBContext>();
                 }
             );
        }
    }
}
