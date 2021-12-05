using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Pages.Admin.Orders
{
    public class ChangeInvoiceNumberModel : PageModel
    {
        public ChangeInvoiceNumberModel(IEntityRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        private readonly IEntityRepository<Order> _orderRepository;

        [BindProperty]
        public Order Order { get; set; }

        [BindProperty]
        [Display(Name = "Номер документа")]
        public int? InvoiceNumber { get; set; }

        public IActionResult OnGet(int id)
        {
            Order = _orderRepository.GetWithDependencies(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            Order = _orderRepository.GetWithDependencies(Order.Id);
            Order.InvoiceNumber = InvoiceNumber;

            Order = _orderRepository.Update(Order);

            return RedirectToPage("/Admin/Orders/Index");
        }
    }
}
