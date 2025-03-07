

using Microsoft.EntityFrameworkCore;
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Models;
using Piloto.Api.Infrastructure.Data;
using Piloto.Api.Infrastructure.Data.Repository;

namespace Piloto.Api.Infrastructure.Repository.Repositories
{
    public class RepositoryProduct : RepositoryBase<Product, DbContext>, IRepositoryProduct

    {
        public RepositoryProduct(IDbFactoryBase<DbContext> dbFactoryBase) : base(dbFactoryBase) { }


    }
}