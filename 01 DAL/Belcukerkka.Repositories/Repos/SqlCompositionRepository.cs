using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Belcukerkka.Repositories.Repos
{
    public class SqlCompositionRepository : SqlEntityRepository<Composition>, IEntityRepository<Composition>
    {
        public SqlCompositionRepository(CandyShopDbContext dbContext)
            : base(dbContext)
        {
        }

        public override Composition GetWithDependencies(int id)
        {
            Composition composition = GetAllWithDependencies().FirstOrDefault(c => c.Id == id);

            return composition;
        }

        public override IEnumerable<Composition> GetAllWithDependencies()
        {
            IEnumerable<Composition> compositions = _dbContext.Compositions
                .Include(c => c.CandiesInComposition)
                .ThenInclude(c => c.Candy)
                .Include(c => c.WeightType)
                .Include(c => c.Boxes);

            return compositions;
        }
    }
}
