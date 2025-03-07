

using Microsoft.EntityFrameworkCore;
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Models;
using Piloto.Api.Infrastructure.Data;
using Piloto.Api.Infrastructure.Data.Repository;

namespace Piloto.Api.Infrastructure.Repository.Repositories
{
    public class RepositorySupplier : RepositoryBase<Supplier, DbContext>, IRepositorySupplier
    {
        public RepositorySupplier(IDbFactoryBase<DbContext> dbFactoryBase) : base(dbFactoryBase) { }
    }

}