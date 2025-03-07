
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Core.Interfaces.Services;
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public async virtual Task<TEntity> Add(TEntity obj)
        {
            if (!obj.IsValid())
                return null;

            return await _repository.Add(obj);
        }
        public async virtual Task AddRange(ICollection<TEntity> objs)
        {
            try
            {
                await _repository.AddRange(objs);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async virtual Task<TEntity> GetById(int id)
        {
            return await _repository.GetById(id);
        }
        public async virtual Task<ICollection<TEntity>> GetAll()
        {
            return await _repository.GetAll();
        }
        public async Task<ICollection<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
             return await _repository.Find(predicate);
        }
        public async virtual Task<TEntity> Update(TEntity obj)
        {
            if (!obj.IsValid())
                return null;

            return await _repository.Update(obj);
        }
        public async virtual Task<int> Remove(TEntity obj)
        {
           return await _repository.Remove(obj);
        }

        public virtual void Dispose()
        {
            _repository.Dispose();
        }

       
    }
}
