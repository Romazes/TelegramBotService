using System.Text.RegularExpressions;
using TelegramBotService.Utilities;

namespace TelegramBotService.Extensions
{
    internal static class Extensions
    {
        /// <summary>
        /// URL-string has checked by rex-pattern
        /// </summary>
        /// <param name="input">URL of web-sites</param>
        /// <returns>true/false</returns>
        public static bool IsValidUrl(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            try
            {
                return Regex.IsMatch(input, Constants.regexUrl);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
