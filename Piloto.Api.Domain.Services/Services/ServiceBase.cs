
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Core.Interfaces.Services;
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Piloto.Api.Domain.Services.Services
{
    public abstract class ServiceBase<TEntity> : IDisposable, IServiceBase<TEntity> 
        where TEntity : Entity

    {
        private readonly IRepositoryBase<TEntity> _repository;
        //private readonly IUnitOfWork<TContext> _unitOfWork;

        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
            //_unitOfWork = unitOfWork;
        }
        public async virtual Task<TEntity> AddAsync(TEntity obj)
        {
            if (!obj.IsValid())
                return null;

            return await _repository.AddAsync(obj);
        }
        public async virtual Task AddRangeAsync(ICollection<TEntity> objs)
        {
            try
            {
                await _repository.AddRangeAsync(objs);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async virtual Task<TEntity> GetByIdAsync(int id, IQueryable<TEntity> query = null, bool asNoTracking = true, bool asSingleQuery = true)
        {
            return await _repository.GetByIdAsync(id, query,asNoTracking,asSingleQuery);
        }
        public async virtual Task<ICollection<TEntity>> GetAsync(
            IQueryable<TEntity> query = null,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asNoTracking = true,
            bool asSingleQuery = true
            )
        {
            return await _repository.GetAsync(query, filter, orderBy,asNoTracking, asSingleQuery);
        }
        public async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
             return await _repository.FindAsync(predicate);
        }
        public async virtual Task<TEntity> UpdateAsync(TEntity obj)
        {
            if (!obj.IsValid())
                return null;

            return await _repository.UpdateAsync(obj);
        }
        public async virtual Task<int> RemoveAync(TEntity obj)
        {
           return await _repository.RemoveAsync(obj);
        }

        public IQueryable<TEntity> GetQuery(params Expression<Func<TEntity, object>>[] includes)
        {
            return _repository.GetQuery(includes);
        }

        public virtual void Dispose()
        {
            _repository.Dispose();
        }

       
    }
}
