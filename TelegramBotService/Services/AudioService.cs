﻿using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService.Extensions;
using TelegramBotService.Services.Interface;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using static TelegramBotService.Utilities.Constants;

namespace TelegramBotService.Services
{
    public class AudioService : IAudioService
    {
        private readonly ILogger<AudioService> _logger;
        private readonly YoutubeClient _youtubeClient;
        private readonly TelegramBotClient _telegramBotClient;

        private long ChatId { get; set; }

        public AudioService(ILogger<AudioService> logger, YoutubeClient youtubeClient,
            TelegramBotClient telegramBotClient)
        {
            _logger = logger;
            _youtubeClient = youtubeClient;
            _telegramBotClient = telegramBotClient;
        }

        public async Task<bool> GetAudio(Update update)
        {
            ChatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;
            YoutubeExplode.Videos.Video videoId;
            StreamManifest streamInfoSet;

            _logger.LogInformation($"Received a text message in chat {ChatId} with message {messageText}");

            if (!messageText.IsValidUrl() | messageText == "/start")
            {
                await SendTextMessage(ValidationMessages.StartCommandReplyMessage);
                return false;
            }

            try
            {
                videoId = await _youtubeClient.Videos.GetAsync(messageText);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error for message {messageText} with GetVideoMediaDataAsync: {ex}");
                await SendTextMessage(ValidationMessages.InvalidLinkMessage);
                return false;
            }

            try
            {
                streamInfoSet = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId.Id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error for message {messageText} with GetVideoMediaStreamInfosAsync: {ex}");
                await SendTextMessage(ValidationMessages.SomeErrorMessage);
                return false;
            }

            var streamInfo = await GetAudioStreamInfo(streamInfoSet, messageText);

            if (streamInfo != null)
            {
                await SendAudio(streamInfo, videoId);
            }

            return true;
        }

        /// <summary>
        /// Send Audio to last destionation (user)
        /// </summary>
        /// <param name="streamInfo">audio file</param>
        /// <param name="video">need for get some information about song.</param>
        /// <returns></returns>
        public async Task SendAudio(IAudioStreamInfo streamInfo, YoutubeExplode.Videos.Video video)
        {
            using (var audioStream = await _youtubeClient.Videos.Streams.GetAsync(streamInfo))
            {
                await _telegramBotClient.SendAudioAsync(
                    chatId: ChatId,
                    audio: audioStream,
                    caption: video.Url,
                    duration: (int)video.Duration.TotalSeconds,
                    performer: video.Author,
                    title: video.Title);
            }
        }

        /// <summary>
        /// Get Audio Stream 
        /// </summary>
        /// <param name="streamManifest">manifest bases on video bitray/quality</param>
        /// <param name="messageText">text which user input, need for exception</param>
        /// <returns></returns>
        private async Task<IAudioStreamInfo> GetAudioStreamInfo(StreamManifest streamManifest, string messageText)
        {
            IAudioStreamInfo streamInfo = (IAudioStreamInfo)streamManifest
                .GetAudioOnly()
                .WithHighestBitrate();

            if (streamInfo == null || streamInfo.Size.TotalMegaBytes > 50)
            {
                var failMessage = streamManifest.GetAudioOnly().Any() ? ValidationMessages.FileSizeExceedLimitMessage : ValidationMessages.Mp4DoesNotExistsMessage;
                await SendTextMessage(failMessage);
                _logger.LogInformation($"Stream info for {messageText} was empty and error for user is {failMessage}");
                return null;
            }

            return streamInfo;
        }

        /// <summary>
        /// Send message to client about different exception or valid input etc.
        /// </summary>
        /// <param name="messageText">text which user input, need for exception</param>
        /// <returns></returns>
        private async Task SendTextMessage(string messageText) => await _telegramBotClient.SendTextMessageAsync(ChatId, messageText);

    }
}
