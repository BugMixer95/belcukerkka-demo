using Belcukerkka.Models.Entities;
using System.Collections.Generic;

namespace Belcukerkka.Repositories.Interfaces
{
    public interface ICatalogItemRepository
    {
        CatalogItem Get(int boxParentId, int weight);
        IEnumerable<CatalogItem> GetAll();
    }
}
