
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Core.Interfaces.Services;
using Piloto.Api.Domain.Models;

namespace Piloto.Api.Domain.Services.Services
{
    public class ServiceSupplier : ServiceBase<Supplier>, IServiceSupplier
    {
        public ServiceSupplier(IRepositorySupplier repository) : base(repository) { }

    }
}