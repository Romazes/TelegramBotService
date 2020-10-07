using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using TelegramBotService.Services.Interface;
using TelegramBotService.Utilities;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace TelegramBotService.Services
{
    public class AudioService : IAudioService
    {
        public TelegramBotClient TelegramClient { get; }
        private readonly ILogger<AudioService> _logger;
        private readonly YoutubeClient _youtubeClient;

        private long ChatId { get; set; }

        public AudioService(YoutubeClient youtubeClient,
            ILogger<AudioService> logger
            /*IOptions<AppSettings> config*/)
        {
            _youtubeClient = youtubeClient;
            _logger = logger;
            TelegramClient = new TelegramBotClient("1246596370:AAFNzzM_HSfZqo999Qbu2wITcY0ycg07dxI");
        }

        public async Task GetAudio(Update update)
        {
            ChatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;
            YoutubeExplode.Videos.Video videoId;
            StreamManifest streamInfoSet;

            _logger.LogInformation($"Received a text message in chat {ChatId} with message {messageText}");

            if (messageText == null || !messageText.Contains("https") | messageText == "/start")
            {
                await SendTextMessage(ValidationMessages.StartCommandReplyMessage);
                return;
            }

            try
            {
                videoId = await _youtubeClient.Videos.GetAsync(messageText);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error for message {messageText} with GetVideoMediaDataAsync: {ex}");
                await SendTextMessage(ValidationMessages.InvalidLinkMessage);
                return;
            }

            try
            {
                streamInfoSet = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId.Id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error for message {messageText} with GetVideoMediaStreamInfosAsync: {ex}");
                await SendTextMessage(ValidationMessages.SomeErrorMessage);
                return;
            }

            IAudioStreamInfo streamInfo = (IAudioStreamInfo)streamInfoSet
                .GetAudioOnly()
                .WithHighestBitrate();

            if (streamInfo == null)
            {
                var failMessage = streamInfoSet.GetAudioOnly().Any() ? ValidationMessages.FileSizeExceedLimitMessage : ValidationMessages.Mp4DoesNotExistsMessage;
                await SendTextMessage(failMessage);
                _logger.LogInformation($"Stream info for {messageText} was empty and error for user is {failMessage}");
                return;
            }

            await SendAudio(streamInfo, videoId);
        }

        private async Task SendAudio(IAudioStreamInfo streamInfo, YoutubeExplode.Videos.Video video)
        {
            using (var audioStream = await _youtubeClient.Videos.Streams.GetAsync(streamInfo))
            {
                await TelegramClient.SendAudioAsync(
                    chatId: ChatId,
                    audio: audioStream,
                    caption: video.Url,
                    duration: (int)video.Duration.TotalSeconds,
                    performer: video.Author,
                    title: video.Title);
            }
        }

        private async Task SendTextMessage(string message) => await TelegramClient.SendTextMessageAsync(ChatId, message);

    }
}
