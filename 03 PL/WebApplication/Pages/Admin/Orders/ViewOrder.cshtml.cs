using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Admin.Orders
{
    public class ViewOrderModel : PageModel
    {
        public ViewOrderModel(IEntityRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        private readonly IEntityRepository<Order> _orderRepository;

        public Order Order { get; set; }

        public void OnGet(int id)
        {
            Order = _orderRepository.GetWithDependencies(id);
        }
    }
}
