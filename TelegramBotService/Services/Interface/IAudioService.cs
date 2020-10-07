using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotService.Services.Interface
{
    public interface IAudioService
    {
        Task GetAudio(Update update);
    }
}
