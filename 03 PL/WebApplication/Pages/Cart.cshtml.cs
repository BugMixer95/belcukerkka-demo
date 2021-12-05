using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Belcukerkka.Models.Entities;
using Belcukerkka.Models.Enums;
using Belcukerkka.Models.ViewModels;
using Belcukerkka.Repositories.Interfaces;
using Belcukerkka.Services;
using Belcukerkka.TelegramNotifier;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages
{
    public class CartModel : PageModel
    {
        private readonly IEntityRepository<Box> _boxRepository;
        private readonly IEntityRepository<Customer> _customerRepository;
        private readonly IEntityRepository<Order> _orderRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CartModel(IEntityRepository<Box> boxRepository,
            IEntityRepository<Customer> customerRepository,
            IEntityRepository<Order> orderRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _boxRepository = boxRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<CartItemViewModel> CartItems { get; set; }

        [BindProperty]
        public Customer Customer { get; set; }

        public int ItemsCount { get; set; }

        public IActionResult OnGet()
        {
            if (_webHostEnvironment.EnvironmentName == "Staging" && !User.Identity.IsAuthenticated)
                return RedirectToPage("/Maintain");

            CartItems = SessionHandler.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "Cart");

            if (CartItems == null)
                CartItems = new List<CartItemViewModel>();

            Customer = new Customer();
            ItemsCount = CartItems.Count();

            return Page();
        }

        public IActionResult OnPostAddToCart(int boxId, int amount)
        {
            var box = _boxRepository.GetWithDependencies(boxId);
            var item = new CartItemViewModel() { Box = box, Amount = amount };

            CartItems = SessionHandler.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "Cart");

            if (CartItems == null)
            {
                CartItems = new List<CartItemViewModel>();
                CartItems.Add(item);
            }
            else
            {
                var index = GetBoxIndex(CartItems, boxId);

                if (index == -1)
                    CartItems.Add(item);
                else
                    CartItems[index].Amount += amount;
            }

            SessionHandler.SetObjectAsJson(HttpContext.Session, "Cart", CartItems);

            return RedirectToPage("/Catalog/Item", new { boxId = boxId });
        }

        public void OnPostRemoveProductFromCart(int itemId)
        {
            CartItems = SessionHandler.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "Cart");
            CartItems.Remove(CartItems[itemId]);

            SessionHandler.SetObjectAsJson(HttpContext.Session, "Cart", CartItems);
        }

        public void OnPostChangeProductQuantity(int itemId, int newQuantity)
        {
            CartItems = SessionHandler.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "Cart");
            CartItems[itemId].Amount = newQuantity;

            SessionHandler.SetObjectAsJson(HttpContext.Session, "Cart", CartItems);
        }

        public async Task<IActionResult> OnPostOrder()
        {
            var customer = _customerRepository.GetAll()
                .Where(c => c.Name == Customer.Name && c.Phone == Customer.Phone && c.Type == Customer.Type)
                .Where(c => c.Address != null && c.Address == Customer.Address)
                .FirstOrDefault();

            if (customer == null)
                customer = _customerRepository.Create(Customer);

            Order order = new Order();
            CartItems = SessionHandler.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "Cart");

            order.Customer = customer;

            if (User.Identity.IsAuthenticated)
                order.CreatedBy = OrderCreatedBy.Admin;
            else
                order.CreatedBy = OrderCreatedBy.User;

            foreach (var item in CartItems)
            {
                var box = _boxRepository.Get(item.Box.Id);

                order.OrderItems.Add(new OrderItem()
                {
                    Box = box,
                    Amount = item.Amount
                });
            }

            order = _orderRepository.Create(order);
            order = _orderRepository.GetWithDependencies(order.Id);

            var notifier = new OrderConfirmedSender();
            await notifier.SendOrderConfirmationMessageAsync(order);

            SessionHandler.ClearSessionObject(HttpContext.Session, "Cart");

            return RedirectToPage("/Catalog/Index");
        }

        public IActionResult OnPostCheckCart()
        {
            CartItems = SessionHandler.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "Cart");

            bool isCartEmpty;

            if (CartItems == null || CartItems?.Count == 0)
                isCartEmpty = true;
            else
                isCartEmpty = false;

            JsonResult json = new JsonResult(new { isEmpty = isCartEmpty });

            return json;
        }

        private int GetBoxIndex(List<CartItemViewModel> list, int boxId)
        {
            int index;

            var checkExistance = list.Any(ci => ci.Box.Id == boxId);

            if (checkExistance)
            {
                var box = list.FirstOrDefault(ci => ci.Box.Id == boxId);
                index = list.IndexOf(box);
            }
            else
                index = -1;

            return index;
        }
    }
}
