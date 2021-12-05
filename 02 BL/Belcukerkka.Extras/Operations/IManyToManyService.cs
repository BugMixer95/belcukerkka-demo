using System.Threading.Tasks;

namespace Belcukerkka.Services.Operations
{
    /// <summary>
    /// Specifies the contract for CRUD operations for entities with many-to-many relation.
    /// </summary>
    public interface IManyToManyService<TEntity> where TEntity : class
    {
        TEntity CreateOnGet(int firstEntityId);
        TEntity FindOnGet(int firstEntityId, int secondEntityId);

        Task<TEntity> CreateOnPostAsync(TEntity entity);
        Task<TEntity> UpdateOnPostAsync(TEntity entity);
        Task<TEntity> DeleteOnPostAsync(int firstEntityId, int secondEntityId);

        bool CheckOperationType(int firstEntityId, int secondEntityId);
    }
}
