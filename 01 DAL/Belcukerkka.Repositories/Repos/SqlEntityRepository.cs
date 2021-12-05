using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Belcukerkka.Repositories.Repos
{
    public class SqlEntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity
    {
        protected readonly CandyShopDbContext _dbContext;

        public SqlEntityRepository(CandyShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TEntity Create(TEntity entityToCreate)
        {
            _dbContext.Set<TEntity>().Add(entityToCreate);
            _dbContext.SaveChanges();

            return entityToCreate;
        }

        public TEntity Delete(int id)
        {
            TEntity entityToDelete = _dbContext.Set<TEntity>().Find(id);

            if (entityToDelete != null)
            {
                _dbContext.Set<TEntity>().Remove(entityToDelete);
                _dbContext.SaveChanges();
            }

            return entityToDelete;
        }

        public TEntity Get(int? id)
        {
            return _dbContext.Set<TEntity>().FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().ToList();
        }

        public TEntity Update(TEntity entityToUpdate)
        {
            var entityEntry = _dbContext.Entry(_dbContext.Set<TEntity>().FirstOrDefault(e => e.Id == entityToUpdate.Id));
            entityEntry.CurrentValues.SetValues(entityToUpdate);

            _dbContext.SaveChanges();

            return entityToUpdate;
        }

        public IEnumerable<TEntity> CreateMultiple(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _dbContext.Set<TEntity>().Add(entity);
            }

            _dbContext.SaveChanges();

            return entities;
        }

        public virtual TEntity GetWithDependencies(int id)
        {
            return Get(id);
        }

        public virtual IEnumerable<TEntity> GetAllWithDependencies()
        {
            return GetAll();
        }

    }
}
