
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Core.Interfaces.Services;
using Piloto.Api.Domain.Models;

namespace Piloto.Api.Domain.Services.Services
{
    public class ServiceProduct : ServiceBase<Product>, IServiceProduct
    {
        private readonly IRepositoryProduct _repository;

        public ServiceProduct(IRepositoryProduct repository) : base(repository) => _repository = repository;

    }
}
