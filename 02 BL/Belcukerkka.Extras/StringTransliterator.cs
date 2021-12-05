using NickBuhro.Translit;

namespace Belcukerkka.Services
{
    /// <summary>
    /// Transliteration of a string from one language to another.
    /// </summary>
    public class StringTransliterator
    {
        /// <summary>
        /// Transliterates specified text from Russian to English letters.
        /// </summary>
        /// <param name="textToConvert">Text that should be transliterated.</param>
        /// <returns>Specified text written in English letters.</returns>
        public static string FromRussianToEnglish(string textToConvert)
        {
            string convertedText = Transliteration.CyrillicToLatin(textToConvert)
                .ToLowerInvariant()
                .Replace(' ','-')
                .Replace("\"", "")
                .Replace("`", "")
                .Replace("'", "");

            return convertedText;
        }
    }
}
