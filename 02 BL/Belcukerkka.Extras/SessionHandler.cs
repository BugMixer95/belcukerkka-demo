using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Belcukerkka.Services
{
    public static class SessionHandler
    {
        /// <summary>
        /// Saves an object as JSON to the current browser session.
        /// </summary>
        /// <param name="session">Current session.</param>
        /// <param name="key">Name of session key that the serialized object should be assosiated with.</param>
        /// <param name="value">Object to serialize into session.</param>
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            var settings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, MaxDepth = 2048 };
            session.SetString(key, JsonConvert.SerializeObject(value, settings));
        }

        /// <summary>
        /// Gets the specified objecy from the current browser session by key.
        /// </summary>
        /// <param name="session">Current session.</param>
        /// <param name="key">Name of session key that should be searched for requested object.</param>
        /// <returns>Default value of used generic object if session doesn't contain any values for specified key; otherwise, object from session.</returns>
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            var settings = new JsonSerializerSettings { MaxDepth = 2048 };
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value, settings);
        }

        /// <summary>
        /// Removes value of specified key from the current browser session.
        /// </summary>
        /// <param name="session">Current session.</param>
        /// <param name="key">Name of session key which values should be removed.</param>
        public static void ClearSessionObject(this ISession session, string key)
        {
            session.Remove(key);
        }
    }
}
