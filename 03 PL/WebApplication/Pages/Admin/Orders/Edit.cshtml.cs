using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Admin.Orders
{
    public class EditModel : PageModel
    {
        public EditModel(IEntityRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        private readonly IEntityRepository<Order> _orderRepository;
        
        [BindProperty]
        public Order Order { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id.HasValue)
                Order = _orderRepository.GetWithDependencies((int)id);
            else
                Order = new Order();

            if (Order == null)
                return RedirectToPage("/Admin/Orders/Index");

            return Page();
        }
    }
}
