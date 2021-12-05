using Belcukerkka.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace Belcukerkka.TelegramNotifier
{
    public class AccessAttemptSender : Sender
    {
        /// <summary>
        /// Sends a Telegram notification about login attempt to devs chats.
        /// </summary>
        /// <param name="isSuccessful">Indicator of whether login attempt is successful or not.</param>
        /// <param name="loginModel">Login View Model (Username, Password).</param>
        /// <param name="loginAttemptTime">Date and time of access.</param>
        /// <param name="ip">IP address of login requester.</param>
        public async Task SendLoginAttemptMessageAsync(bool isSuccessful, LoginViewModel loginModel, DateTime loginAttemptTime, string ip)
        {
            // \u2705 - heavy check mark
            // \u274C - cross mark

            string message = isSuccessful switch
            {
                true => "\u2705 *ВХОД В СИСТЕМУ* \u2705\n",
                false => $"\u274C *ПОПЫТКА ВХОДА* \u274C\n"
            };

            message += $"*IP*: _{ip}_\n" +
                $"*Логин*: _{loginModel.LoginName}_\n" +
                $"*Пароль*: _{loginModel.Password}_\n" +
                $"*Время*: _{loginAttemptTime}_";

            await Task.Run(() => SendMessageAsync(message, NotificationReceivers.Developers));
        }
    }
}
