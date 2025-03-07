using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.Data.Repository
{
    public class DbFactoryBase<TContext> : IDbFactoryBase<TContext> where TContext : DbContext
    
    {
        private bool _disposed;
        private readonly Func<TContext> _instanceFunc;
        //private readonly MySQLStockManagementDBContext _instanceFunc;
        //private MySQLStockManagementDBContext _context;
        private TContext _context;

        public TContext Context => _context ?? (_context = _instanceFunc.Invoke());
        //public DbContext Context => _context ?? (_context = _instanceFunc);
        public DbFactoryBase(Func<TContext> dbContextFactory)
        //public DbFactoryBase(TContext dbContextFactory)
        //public DbFactoryBase(MySQLStockManagementDBContext dbContextFactory)

        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (!_disposed && _context != null)
            {
                _disposed = true;
                _context.Dispose();
            }
        }
    }

}
