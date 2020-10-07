using Telegram.Bot;

namespace TelegramBotService
{
    public class Audiobot
    { 
        public string BotToken { get; set; }
        public string Username { get; set; }
        public static string Url { get; set; } = "https://7ae826f4dd72.ngrok.io/{0}";
    }
}
