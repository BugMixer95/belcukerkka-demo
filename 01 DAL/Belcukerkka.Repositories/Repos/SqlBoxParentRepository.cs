using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Belcukerkka.Repositories.Repos
{
    public class SqlBoxParentRepository : SqlEntityRepository<BoxParent>, IEntityRepository<BoxParent>
    {
        public SqlBoxParentRepository(CandyShopDbContext dbContext)
            : base(dbContext)
        {
        }

        public override BoxParent GetWithDependencies(int id)
        {
            BoxParent boxParent = GetAllWithDependencies().FirstOrDefault(bp => bp.Id == id);

            return boxParent;
        }

        public override IEnumerable<BoxParent> GetAllWithDependencies()
        {
            using (_dbContext)
            {
                IEnumerable<BoxParent> boxParents = _dbContext.BoxParents
                    .Include(bp => bp.BoxPackage)
                    .Include(bp => bp.Boxes)
                    .ThenInclude(b => b.Composition)
                    .ThenInclude(c => c.CandiesInComposition)
                    .ThenInclude(cic => cic.Candy)
                    .Include(bp => bp.Boxes)
                    .ThenInclude(b => b.Composition)
                    .ThenInclude(c => c.WeightType);

                return boxParents;
            }
        }
    }
}
