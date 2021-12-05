using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Belcukerkka.Repositories.Repos
{
    public class SqlCandyRepository : SqlEntityRepository<Candy>, IEntityRepository<Candy>
    {
        public SqlCandyRepository(CandyShopDbContext dbContext)
            : base(dbContext)
        {
        }

        public override Candy GetWithDependencies(int id)
        {
            Candy candy = _dbContext.Candies
                .Include(c => c.CandyInCompositions)
                .ThenInclude(cic => cic.Composition)
                .FirstOrDefault(c => c.Id == id);

            return candy;
        }
    }
}
