using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belcukerkka.Repositories.Repos
{
    public class SqlCatalogItemRepository : ICatalogItemRepository
    {
        private readonly CandyShopDbContext _dbContext;

        public SqlCatalogItemRepository(CandyShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CatalogItem Get(int boxParentId, int weight)
        {
            return GetAll().FirstOrDefault(ci => ci.Id == boxParentId && ci.Weight == weight);
        }

        public IEnumerable<CatalogItem> GetAll()
        {
            return _dbContext.CatalogItems
                .FromSqlRaw("bel_sp_GetAllCatalogItems")
                .AsNoTracking()
                .ToList()
                .OrderBy(ci => ci.Weight);
        }
    }
}
