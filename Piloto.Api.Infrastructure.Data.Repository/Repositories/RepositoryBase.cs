
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

        public async virtual Task<TEntity> Add(TEntity obj)
        {
            try
            {
                //_context.Set<TEntity>().Add(obj);
                //_context.SaveChanges();
                var result = await DbSet.AddAsync(obj);
                return result.Entity;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async virtual Task AddRange(ICollection<TEntity> objs)
        {
            try
            {
                //_context.Set<TEntity>().Add(obj);
                //_context.SaveChanges();
                 await DbSet.AddRangeAsync(objs);
             

            }
            catch (Exception)
            {

                throw;
            }
        }
        //public virtual TEntity GetById(int Id) => _context.Set<TEntity>().Find(Id);
        //public virtual ICollection<TEntity> GetAll() => _context.Set<TEntity>().ToList();
        public async virtual Task<TEntity> GetById(int Id) => await DbSet.FindAsync(Id);
        public async virtual Task<ICollection<TEntity>> GetAll() => await DbSet.ToListAsync();

        //public ICollection<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        //{
        //    var result = _context.Set<TEntity>().Where(predicate);
        //    if (result != null)
        //        return result.ToList();

        //    return null;

        //}
        public async Task<ICollection<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public async virtual Task<TEntity> Update(TEntity obj)
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
        public async virtual Task<int> Remove(TEntity obj)
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

        public virtual void Dispose()
        {
            _dbFactory.Dispose();
        }


    }
}
