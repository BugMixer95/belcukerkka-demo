using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Belcukerkka.Services.Operations
{
    /// <summary>
    /// Specifies CRUD operations for Candy and Composition entities, as well as their many-to-many representation, CandyInComposition.
    /// </summary>
    public class CandyInCompositionService : IManyToManyService<CandyInComposition>
    {
        private readonly CandyShopDbContext _dbContext;

        public CandyInCompositionService(CandyShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates new CandyInComposition record in the database.
        /// </summary>
        /// <param name="candyInComposition">CandyInComposition object that should be created.</param>
        public async Task<CandyInComposition> CreateOnPostAsync(CandyInComposition candyInComposition)
        {
            using (_dbContext)
            {
                var composition = await _dbContext.Compositions
                    .Include(c => c.CandiesInComposition)
                    .ThenInclude(cic => cic.Candy)
                    .FirstOrDefaultAsync(c => c.Id == candyInComposition.CompositionId);

                candyInComposition.Composition = composition;
                composition.CandiesInComposition.Add(candyInComposition);

                var candy = await _dbContext.Candies
                    .Include(c => c.CandyInCompositions)
                    .ThenInclude(cic => cic.Candy)
                    .FirstOrDefaultAsync(c => c.Id == candyInComposition.CandyId);

                candyInComposition.Candy = candy;
                candy.CandyInCompositions.Add(candyInComposition);

                await _dbContext.SaveChangesAsync();

                return candyInComposition;
            }
        }

        /// <summary>
        /// Updates a CandyInComposition record in the database.
        /// </summary>
        /// <param name="candyInComposition">CandyInComposition object that should be updated.</param>
        public async Task<CandyInComposition> UpdateOnPostAsync(CandyInComposition candyInComposition)
        {
            using (_dbContext)
            {
                var composition = await _dbContext.Compositions
                    .Include(c => c.CandiesInComposition)
                    .ThenInclude(cic => cic.Candy)
                    .FirstOrDefaultAsync(c => c.Id == candyInComposition.CompositionId);

                candyInComposition.Composition = composition;
                var candyToDelete = composition.CandiesInComposition.Find(cic => cic.CandyId == candyInComposition.CandyId);
                composition.CandiesInComposition.Remove(candyToDelete);
                composition.CandiesInComposition.Add(candyInComposition);

                var candy = await _dbContext.Candies
                    .Include(c => c.CandyInCompositions)
                    .ThenInclude(cic => cic.Candy)
                    .FirstOrDefaultAsync(c => c.Id == candyInComposition.CandyId);

                candyInComposition.Candy = candy;
                candyToDelete = candy.CandyInCompositions.Find(cic => cic.CompositionId == candyInComposition.CompositionId);
                candy.CandyInCompositions.Remove(candyToDelete);
                candy.CandyInCompositions.Add(candyInComposition);

                await _dbContext.SaveChangesAsync();

                return candyInComposition;
            }
        }

        /// <summary>
        /// Deletes a CandyInComposition record from the database with specified Composition and Candy IDs.
        /// </summary>
        /// <param name="compositionId">ID of composition to search for.</param>
        /// <param name="candyId">ID of candy to search for.</param>
        public async Task<CandyInComposition> DeleteOnPostAsync(int compositionId, int candyId)
        {
            using (_dbContext)
            {
                var candyInComposition = _dbContext.Compositions
                    .Include(c => c.CandiesInComposition)
                    .ThenInclude(c => c.Candy)
                    .FirstOrDefault(c => c.Id == compositionId)
                    .CandiesInComposition.Find(cic => cic.CandyId == candyId);

                var candy = await _dbContext.Candies.FindAsync(candyId);
                candy.CandyInCompositions.Remove(candyInComposition);

                var composition = await _dbContext.Compositions.FindAsync(compositionId);
                composition.CandiesInComposition.Remove(candyInComposition);

                await _dbContext.SaveChangesAsync();

                return candyInComposition;
            }
        }

        /// <summary>
        /// Creates new CandyInComposition object with specified composition ID.
        /// </summary>
        /// <param name="compositionId">ID of composition which a candy will be related to.</param>
        public CandyInComposition CreateOnGet(int compositionId)
        {
            return new CandyInComposition { CompositionId = compositionId };
        }

        /// <summary>
        /// Gets a CandyInComposition object with specified Composition and Candy IDs.
        /// </summary>
        /// <param name="compositionId">ID of composition to search for.</param>
        /// <param name="candyId">ID of candy to search for.</param>
        public CandyInComposition FindOnGet(int compositionId, int candyId)
        {
            using (_dbContext)
            {
                var candyInComposition = _dbContext.Compositions
                    .Include(c => c.CandiesInComposition)
                    .ThenInclude(c => c.Candy)
                    .FirstOrDefault(c => c.Id == compositionId)
                    .CandiesInComposition.Find(cic => cic.CandyId == candyId);

                return candyInComposition;
            }
        }

        /// <summary>
        /// Checks whether the requested operation has 'Edit' type.
        /// </summary>
        /// <param name="compositionId">ID number of composition.</param>
        /// <param name="candyId">ID number of Candy.</param>
        /// <returns>True, if the operation type is 'Edit'; otherwise, false.</returns>
        public bool CheckOperationType(int compositionId, int candyId)
        {
            using (_dbContext)
            {
                var composition = _dbContext.Compositions
                    .Include(c => c.CandiesInComposition)
                    .FirstOrDefault(c => c.Id == compositionId);

                bool isEditOperation = composition.CandiesInComposition.Exists(cic => cic.CandyId == candyId);

                return isEditOperation;
            }
        }
    }
}
