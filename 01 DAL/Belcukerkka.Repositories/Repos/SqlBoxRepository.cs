using Belcukerkka.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Belcukerkka.Repositories.Repos
{
    public class SqlBoxRepository : SqlEntityRepository<Box>
    {
        public SqlBoxRepository(CandyShopDbContext dbContext)
            : base(dbContext)
        {
        }

        public override Box GetWithDependencies(int id)
        {
            Box box = GetAllWithDependencies().FirstOrDefault(b => b.Id == id);

            return box;
        }

        public override IEnumerable<Box> GetAllWithDependencies()
        {
            IEnumerable<Box> boxes = _dbContext.Boxes
                .Include(b => b.BoxParent)
                .ThenInclude(bp => bp.BoxPackage)
                .Include(b => b.Composition)
                .ThenInclude(c => c.WeightType)
                .Include(b => b.Composition)
                .ThenInclude(c => c.CandiesInComposition)
                .ThenInclude(cic => cic.Candy);

            return boxes;
        }

        //public IEnumerable<Box> Search(string searchTerm)
        //{
        //    if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrWhiteSpace(searchTerm))
        //    {
        //        return _dbContext.Boxes.ToList();
        //    }

        //    return _dbContext.Boxes.Where(b => b.Name.Contains(searchTerm)).ToList();
        //}
    }
}
