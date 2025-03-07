using Microsoft.Extensions.DependencyInjection;
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Infrastructure.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.Data.Repository
{
    public static class RepositoryServicesDI
    {
        public static void AddRepositoryServicesDI(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDbFactoryBase<>), typeof(DbFactoryBase<>));
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped<IRepositorySupplier, RepositorySupplier>();
            services.AddScoped<IRepositoryProduct, RepositoryProduct>();
            services.AddScoped<IRepositoryProductSupplier, RepositoryProductSupplier>();
        }
    }
}
