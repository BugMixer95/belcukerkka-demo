using Belcukerkka.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Belcukerkka.Repositories.Repos
{
    public class SqlOrderRepository : SqlEntityRepository<Order>
    {
        public SqlOrderRepository(CandyShopDbContext dbContext)
            : base(dbContext)
        {
        }

        public override Order GetWithDependencies(int id)
        {
            return GetAllWithDependencies().FirstOrDefault(o => o.Id == id);
        }

        public override IEnumerable<Order> GetAllWithDependencies()
        {
            IEnumerable<Order> orders = _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Box)
                .ThenInclude(b => b.BoxParent)
                .ThenInclude(bp => bp.BoxPackage)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Box)
                .ThenInclude(b => b.Composition)
                .ThenInclude(c => c.WeightType)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Box)
                .ThenInclude(b => b.Composition)
                .ThenInclude(c => c.CandiesInComposition)
                .ThenInclude(cic => cic.Candy);

            return orders;
        }
    }
}
