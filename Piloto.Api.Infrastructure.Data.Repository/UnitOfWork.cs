using Microsoft.EntityFrameworkCore;
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.Data.Repository
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
   
    {
        private IDbFactoryBase<TContext> _dbFactory;


        public UnitOfWork(IDbFactoryBase<TContext> dbFactory)
        
        {
            _dbFactory = dbFactory;
        }

        public TContext Context => _dbFactory.Context;
      

        public Task<int> SaveChangeAsync()
        {
            return Context.SaveChangesAsync();
        }
        public async Task<TResult> ExecuteTransactionAsync<TResult>(Func<Task<TResult>> func)
        {
            var strategy = Context.Database.CreateExecutionStrategy();
            var transResult = await strategy.ExecuteAsync(async () =>
            {
                using (var trans = await Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var result = await func.Invoke();
                        await trans.CommitAsync();
                        return result;
                    }
                    catch (Exception)
                    {
                        await trans.RollbackAsync();
                        throw;
                    }
                }
            });
            return transResult;
        }
    }
}
