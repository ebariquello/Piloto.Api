using Piloto.Api.Application.Interfaces;
using Piloto.Api.Application.Services;
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Core.Interfaces.Services;
using Piloto.Api.Domain.Services.Services;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Interfaces;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Map;
//using Piloto.Api.Infrastructure.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;


using AutoMapper;
using Piloto.Api.Infrastructure.Data.Repository;
using Piloto.Api.Infrastructure.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Piloto.Api.Infrastructure.Data;
using System;
using Microsoft.Extensions.Configuration;

namespace Piloto.Api.Infrastructure.CrossCutting.Adapter.IOC;
public static class ConfigurationIOC
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration )
    {
        #region Registra IOC
        

        #region IOC Mapper
        
        #endregion

        #region IOC Repositorys SQL
        #endregion

        #region IOC Services
        #endregion

        #region IOC Application
        #endregion
        #endregion

    }

}
