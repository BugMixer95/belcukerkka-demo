using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Belcukerkka.Services.Extras
{
    public class CompositionDropdownHandler : DropdownHandler
    {
        /// <summary>
        /// Gets list of all available weights from the database.
        /// </summary>
        /// <param name="repo">Composition repository.</param>
        /// <returns>IEnumerable representation of all available weights.</returns>
        public static IEnumerable<SelectListItem> GetDistinctedWeightsSelectList(IEntityRepository<Composition> repo)
        {
            var list = new List<SelectListItem>();
            var compositions = repo.GetAll().Select(c => c.Weight).Distinct();

            foreach (var item in compositions)
            {
                list.Add(new SelectListItem(item.ToString(), item.ToString()));
            }

            return list;
        }


        /// <summary>
        /// Gets all the available weight types from the database except for those that already exist for specified box.
        /// </summary>
        /// <param name="boxRepo">Box repository.</param>
        /// <param name="compositionRepo">Composition repository.</param>
        /// <param name="requestBody"></param>
        /// <returns>IEnumerable representation of all available weight types.</returns>
        public static IEnumerable<SelectListItem> GetWeightTypeNames(IEntityRepository<Box> boxRepo,
            IEntityRepository<Composition> compositionRepo,
            string requestBody)
        {
            var json = JsonConvert.DeserializeObject<JsonBoxWeightParameters>(requestBody);
            
            var listAllNames = compositionRepo.GetAllWithDependencies()
                .Where(c => c.Weight == int.Parse(json.weight))
                .Select(
                    c => new
                    {
                        WeightTypeId = c.Id.ToString(),
                        Name = c.WeightType.Name
                    });

            var listFilteredNames = boxRepo.GetAllWithDependencies()
                .Where(b => b.Composition.Weight == int.Parse(json.weight))
                .Where(b => b.BoxParentId == int.Parse(json.boxParentId))
                .Select(
                    b => new
                    {
                        WeightTypeId = b.Composition.Id.ToString(),
                        Name = b.Composition.WeightType.Name
                    });

            IEnumerable<SelectListItem> listToSend = listAllNames
                .Except(listFilteredNames)
                .Select(
                    x => new SelectListItem
                    {
                        Value = x.WeightTypeId,
                        Text = x.Name
                    }
                );

            return listToSend;
        }
    }
}
