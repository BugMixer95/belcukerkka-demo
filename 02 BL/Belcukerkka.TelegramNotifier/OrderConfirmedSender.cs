using Belcukerkka.Models.Entities;
using Belcukerkka.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Belcukerkka.TelegramNotifier
{
    public class OrderConfirmedSender : Sender
    {
        /// <summary>
        /// Sends a Telegram notification about new order to managers chats.
        /// </summary>
        /// <param name="order">Order model.</param>
        /// <returns></returns>
        public async Task SendOrderConfirmationMessageAsync(Order order)
        {
            string empty = "_не указано_";

            if (order.CreatedBy == OrderCreatedBy.User)
            {
                string message = "*НОВЫЙ ЗАКАЗ*\n";

                string name = order.Customer.Type == CustomerType.LegalEntity ? "Название компании" : "ФИО";

                message += $"*{name}*: {order.Customer.Name}\n";

                if (order.Customer.Type == CustomerType.LegalEntity)
                    message += $"*ФИО контактного лица*: {order.Customer.ContactPerson}\n";

                message += $"*Телефон*: {order.Customer.Phone.Replace("+", "%2b")}\n" +
                $"*Адрес*: {order.Customer.Address ?? empty}\n" +
                $"*Сумма заказа*: {order.OrderItems.Sum(oi => oi.Box.Price * oi.Amount):0.00}\n";

                message += "\n";

                string orderItems = "*ДЕТАЛИ ЗАКАЗА*: \n";

                foreach (var item in order.OrderItems)
                {
                    orderItems += $"{item.Box.BoxParent.Name}, {item.Box.Composition.WeightType.Name}, {item.Box.Composition.Weight} гр." +
                        $" - " +
                        $"{item.Amount} шт.\n";
                }

                message += orderItems;

                await Task.Run(() => SendMessageAsync(message, NotificationReceivers.Managers));
            }
        }
    }
}
