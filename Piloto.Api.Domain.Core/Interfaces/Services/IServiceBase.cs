
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Piloto.Api.Domain.Core.Interfaces.Services
{
    public interface IServiceBase<TEntity> 
        where TEntity : Entity
    {

        Task<TEntity> Add(TEntity entity);

        Task AddRange(ICollection<TEntity> entities);


        Task<TEntity> GetById(int id);

        Task<ICollection<TEntity>> GetAll();

        Task<ICollection<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> Update(TEntity entity);

        Task<int> Remove(TEntity entity);

        void Dispose();
    }
}