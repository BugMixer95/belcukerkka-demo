using Belcukerkka.Models.Entities;

namespace Belcukerkka.Repositories.Repos
{
    public class SqlOrderItemRepository : SqlEntityRepository<OrderItem>
    {
        public SqlOrderItemRepository(CandyShopDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
