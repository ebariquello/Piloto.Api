

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.Data.Repository
{
    public interface IDbFactoryBase<TContext> : IDisposable where TContext : DbContext
    {
        TContext Context { get; }
    }
}
