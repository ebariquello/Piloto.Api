using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Interfaces;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.CrossCutting.Adapter
{
    public static class CrossCuttingAdapterServicesDI
    {
        public static void AddCrossCuttingAdapterServicesDI(this IServiceCollection services)
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingConfiguration());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<IMapperSupplier, MapperSupplier>();
            services.AddScoped<IMapperProduct, MapperProduct>();
        }
    }
}
