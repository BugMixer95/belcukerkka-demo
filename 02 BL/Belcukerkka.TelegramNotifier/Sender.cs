using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Belcukerkka.TelegramNotifier
{
    public class Sender
    {
        /// <summary>
        /// Sends specified message to requested chats.
        /// </summary>
        /// <param name="message">String representation of message that is needed to be send.</param>
        /// <param name="receivers">Chats that the message should be sent to.</param>
        /// <returns></returns>
        protected async Task SendMessageAsync(string message, NotificationReceivers receivers)
        {
            var token = Configuration.Token;
            
            List<string> chats = receivers switch
            {
                NotificationReceivers.Developers => Configuration.DevChats,
                NotificationReceivers.Managers => Configuration.ManagerChats,
                _ => throw new ArgumentException("Неверное значение!")
            };

            string url;

            foreach (var chat in chats)
            {
                WebClient client = new WebClient();

                url = $"https://api.telegram.org/bot{token}/sendMessage?chat_id={chat}&parse_mode=Markdown&text={message}";
                Uri uri = new Uri(url);

                await Task.Run(() => client.DownloadStringAsync(uri));
            }
        }
    }
}
