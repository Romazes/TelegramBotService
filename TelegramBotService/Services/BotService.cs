using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBotService.Services
{
    public class BotService
    {
        private static TelegramBotClient botClient;

        /// <summary>
        /// Invoke Telegram Client 
        /// </summary>
        /// <returns></returns>
        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
                return botClient;

            botClient = new TelegramBotClient(Startup.StaticConfig["BotToken"]);
            string hook = string.Format(Audiobot.Url, "api/audio/update");
            await botClient.SetWebhookAsync(hook);
            return botClient;
        }
    }
}
