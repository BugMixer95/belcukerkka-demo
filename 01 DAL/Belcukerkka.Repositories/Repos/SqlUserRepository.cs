using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;

namespace Belcukerkka.Repositories.Repos
{
    public class SqlUserRepository : SqlEntityRepository<User>, IEntityRepository<User>
    {
        public SqlUserRepository(CandyShopDbContext dbContext)
            : base (dbContext)
        {
        }
    }
}
