using Belcukerkka.Models.Entities;

namespace Belcukerkka.Repositories.Repos
{
    public class SqlCustomerRepository : SqlEntityRepository<Customer>
    {
        public SqlCustomerRepository(CandyShopDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
