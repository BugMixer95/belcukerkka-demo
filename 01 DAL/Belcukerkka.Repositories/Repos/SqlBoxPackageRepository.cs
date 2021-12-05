using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;

namespace Belcukerkka.Repositories.Repos
{
    public class SqlBoxPackageRepository : SqlEntityRepository<BoxPackage>, IEntityRepository<BoxPackage>
    {
        public SqlBoxPackageRepository(CandyShopDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
