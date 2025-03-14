
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Piloto.Api.Domain.Core.Interfaces.Services
{
    public interface IServiceBase<TEntity> 
        where TEntity : Entity
    {

        Task<TEntity> AddAsync(TEntity entity);

        Task AddRangeAsync(ICollection<TEntity> entities);


        Task<TEntity> GetByIdAsync(int id, IQueryable<TEntity> query = null, bool asNoTracking = true, bool asSingleQuery = true);

        Task<ICollection<TEntity>> GetAsync(
            IQueryable<TEntity> query = null,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asNoTracking = true,
            bool asSingleQuery = true
            );

        Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<int> RemoveAync(TEntity entity);

        IQueryable<TEntity> GetQuery(params Expression<Func<TEntity, object>>[] includes);

        void Dispose();
    }
}