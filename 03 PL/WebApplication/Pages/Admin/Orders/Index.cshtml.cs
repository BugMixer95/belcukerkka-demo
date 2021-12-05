using System.Collections.Generic;
using System.Linq;
using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        public IndexModel(IEntityRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        private readonly IEntityRepository<Order> _orderRepository;

        public IEnumerable<Order> Orders { get; set; }

        public void OnGet()
        {
            Orders = _orderRepository.GetAllWithDependencies().OrderByDescending(o => o.Date);
        }
    }
}
