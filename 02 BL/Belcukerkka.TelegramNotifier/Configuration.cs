using System.Collections.Generic;

namespace Belcukerkka.TelegramNotifier
{
    /// <summary>
    /// Specifies the configuration used for Telegram Sender work.
    /// </summary>
    internal class Configuration
    {
        /// <summary>
        /// API Token of created Telegram bot.
        /// </summary>
        internal static string Token { get; } = "ENTER YOUR TELEGRAM BOT API TOKEN AND IT WILL WORK";

        /// <summary>
        /// List of developer/support guys chats.
        /// </summary>
        internal static List<string> DevChats { get; } = new List<string>
        {
            // here go some dev chats
        };

        /// <summary>
        /// List of manager chats.
        /// </summary>
        internal static List<string> ManagerChats { get; } = new List<string>
        {
            // here go some manager chats
        };
    }
}
