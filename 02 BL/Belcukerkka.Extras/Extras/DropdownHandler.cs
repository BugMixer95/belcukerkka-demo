using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Belcukerkka.Services.Extras
{
    public class DropdownHandler
    {
        /// <summary>
        /// Creates a list of values that will be presented in a dropdown list from specified entity repository and property name.
        /// </summary>
        /// <param name="repo">Repository to use.</param>
        /// <param name="propertyName">Name of property that the final list should have values from.</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectList<TEntity>(IEntityRepository<TEntity> repo, string propertyName)
            where TEntity : Entity
        {
            var list = new List<SelectListItem>();
            var entities = repo.GetAll();

            foreach (var item in entities)
            {
                var propertyInfo = item.GetType().GetProperty(propertyName);
                var propertyValue = propertyInfo.GetValue(item, null).ToString();

                list.Add(new SelectListItem(propertyValue, item.Id.ToString()));
            }

            return list;
        }
    }
}
