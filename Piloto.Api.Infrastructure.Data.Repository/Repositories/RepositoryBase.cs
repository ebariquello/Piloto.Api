
using Microsoft.EntityFrameworkCore;
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Models;
using Piloto.Api.Infrastructure.Data;
using Piloto.Api.Infrastructure.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.Repository.Repositories
{
    public class RepositoryBase<TEntity, TContext> : IDisposable, IRepositoryBase<TEntity>
  
        where TEntity : Entity
        where TContext : DbContext
    {
        private readonly IDbFactoryBase<TContext> _dbFactory;
       
        private readonly string _sharedEntityName = string.Empty;
        protected DbSet<TEntity> _dbSet;
        protected DbSet<TEntity> DbSet
        {
            get {
                if(string.IsNullOrEmpty(_sharedEntityName))
                return _dbSet ?? (_dbSet = _dbFactory.Context.Set<TEntity>());
                else return _dbSet ?? (_dbSet = _dbFactory.Context.Set<TEntity>(_sharedEntityName));
            }
        }
        public RepositoryBase(IDbFactoryBase<TContext> dbFactory)

        {
            _dbFactory = dbFactory;
           
        }

        public async virtual Task<TEntity> AddAsync(TEntity obj)
        {
            try
            {
                var result = await DbSet.AddAsync(obj);
                return result.Entity;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async virtual Task AddRangeAsync(ICollection<TEntity> objs)
        {
            try
            {
                 await DbSet.AddRangeAsync(objs);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public virtual async Task<TEntity> GetByIdAsync(int id, IQueryable<TEntity> query = null, bool asNoTracking = true, bool asSingleQuery = true)
        {
            var entityType = _dbFactory.Context.Model.FindEntityType(typeof(TEntity));
            var _primaryKeyName = entityType.FindPrimaryKey().Properties
                .Select(p => p.Name)
                .FirstOrDefault();

            IQueryable<TEntity> queryAux = query ?? DbSet;

            if (asNoTracking)
            {
                queryAux = queryAux.AsNoTracking();
            }

            if (asSingleQuery)
            {
                queryAux = queryAux.AsSingleQuery();
            }
            return await queryAux.FirstOrDefaultAsync(e => EF.Property<int>(e, _primaryKeyName) == id);
        }


       

        public virtual async Task<ICollection<TEntity>> GetAsync(
            IQueryable<TEntity> query = null,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asNoTracking = true,
            bool asSingleQuery = true
            )
        {
            IQueryable<TEntity> queryAux = query ?? DbSet;

            if (asNoTracking)
            {
                queryAux = queryAux.AsNoTracking();
            }

            if (asSingleQuery)
            {
                queryAux = queryAux.AsSingleQuery();
            }

            if (filter != null)
            {
                queryAux = queryAux.Where(filter);
            }

            if (orderBy != null)
            {
                return await orderBy(queryAux).ToListAsync();
            }
            else
            {
                return await queryAux.ToListAsync();
            }
        }

        public async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public async virtual Task<TEntity> UpdateAsync(TEntity obj)
        {

            try
            {
                var result = await Task.FromResult(DbSet.Update(obj));
                return result.Entity;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public async virtual Task<int> RemoveAsync(TEntity obj)
        {
            try
            {
                //_context.Set<TEntity>().Remove(obj);
                //_context.SaveChanges();
                var result = await Task.FromResult(DbSet.Remove(obj));
                return result.State == EntityState.Deleted? 1: 0;
                //DbSet.Remove(obj);

            }
            catch (Exception)
            {

                throw;
            }


        }
        public IQueryable<TEntity> GetQuery(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query;
        }

        public virtual void Dispose()
        {
            _dbFactory.Dispose();
        }


    }
}
