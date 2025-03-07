using Microsoft.Extensions.DependencyInjection;
using Piloto.Api.Domain.Core.Interfaces.Services;
using Piloto.Api.Domain.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Domain.Services
{
    public static class ServicesDI
    {
        public static void AddServicesDI(this IServiceCollection services)
        {
            services.AddScoped<IServiceSupplier, ServiceSupplier>();
            services.AddScoped<IServiceProduct, ServiceProduct>();
            services.AddScoped<IServiceProductSupplier, ServiceProductSupplier>();

        }
    }
}
