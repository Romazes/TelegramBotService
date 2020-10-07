using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBotService.Services
{
    public class BotService /*: IBotService*/
    {
        private static TelegramBotClient botClient;

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }

            botClient = new TelegramBotClient("1246596370:AAFNzzM_HSfZqo999Qbu2wITcY0ycg07dxI");
            string hook = string.Format(Audiobot.Url, "api/audio/update");
            await botClient.SetWebhookAsync(hook);
            return botClient;
        }
    }
}
