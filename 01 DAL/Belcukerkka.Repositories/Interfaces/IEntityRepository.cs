using Belcukerkka.Models.Entities;
using System.Collections.Generic;

namespace Belcukerkka.Repositories.Interfaces
{
    public interface IEntityRepository<TEntity> where TEntity : Entity
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllWithDependencies();
        TEntity Create(TEntity entityToCreate);
        TEntity Get(int? id);
        TEntity GetWithDependencies(int id);
        TEntity Update(TEntity entityToUpdate);
        TEntity Delete(int id);
    }
}
