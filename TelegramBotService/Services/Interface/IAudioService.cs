using System.Threading.Tasks;
using Video = YoutubeExplode.Videos.Video;
using YoutubeExplode.Videos.Streams;
using Telegram.Bot.Types;

namespace TelegramBotService.Services.Interface
{
    public interface IAudioService
    {
        Task<bool> GetAudio(Update update);
        Task SendAudio(IAudioStreamInfo streamInfo, Video video);
    }
}
