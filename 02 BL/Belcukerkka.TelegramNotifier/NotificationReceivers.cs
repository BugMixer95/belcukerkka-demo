namespace Belcukerkka.TelegramNotifier
{
    /// <summary>
    /// Types of chats that should receive a notification.
    /// </summary>
    public enum NotificationReceivers : byte
    {
        /// <summary>
        /// Devs, admins, supportmen. Anyone who has access to the code.
        /// </summary>
        Developers,
        
        /// <summary>
        /// Managers, salesmen.
        /// </summary>
        Managers
    }
}
