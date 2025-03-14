

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.Data.Repository
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        Task<int> SaveChangeAsync(bool changeEntityTracker = false);

        Task<TResult> ExecuteTransactionAsync<TResult>(Func<Task<TResult>> func);

        TContext Context { get; }
    }
    //public interface IUnitOfWork
    //{
    //    Task<int> SaveChangeAsync();

    //    Task<TResult> ExecuteTransactionAsync<TResult>(Func<Task<TResult>> func);

    //    DbContext Context { get; }
    //}
}
