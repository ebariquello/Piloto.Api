using Microsoft.Extensions.DependencyInjection;
using Piloto.Api.Application.Interfaces;
using Piloto.Api.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Application
{
    public static class ApplicationServicesDI
    {
        public static void AddApplicationServicesDI(this IServiceCollection services) {
            services.AddScoped<IApplicationServiceSupplier, ApplicationServiceSupplier>();
            services.AddScoped<IApplicationServiceProduct, ApplicationServiceProduct>();

        }
    }
}
