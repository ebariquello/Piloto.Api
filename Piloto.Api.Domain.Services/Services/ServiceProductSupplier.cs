using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Core.Interfaces.Services;
using Piloto.Api.Domain.Models;

namespace Piloto.Api.Domain.Services.Services
{
    public class ServiceProductSupplier : ServiceBase<ProductSupplier>, IServiceProductSupplier
    {
        private readonly IRepositoryProductSupplier _repository;

        public ServiceProductSupplier(IRepositoryProductSupplier repository) : base(repository) => _repository = repository;

    }
}
