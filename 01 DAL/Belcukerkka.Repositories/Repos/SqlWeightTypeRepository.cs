using Belcukerkka.Models.Entities;
using System.Collections.Generic;

namespace Belcukerkka.Repositories.Repos
{
    public class SqlWeightTypeRepository : SqlEntityRepository<WeightType>
    {
        public SqlWeightTypeRepository(CandyShopDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
