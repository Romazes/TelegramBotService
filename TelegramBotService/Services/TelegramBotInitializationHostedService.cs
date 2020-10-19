using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBotService.Extensions;

namespace TelegramBotService.Services
{
    public class TelegramBotInitializationHostedService : IHostedService
    {
        private readonly TelegramBotClient _telegramBotClient;
        private readonly AudioBotSettings _options;
        private readonly ILogger<AudioService> _logger;

        public TelegramBotInitializationHostedService(IOptions<AudioBotSettings> options, 
            TelegramBotClient telegramBotClient, ILogger<AudioService> logger)
        {
            _logger = logger;
            _options = options.Value;
            _telegramBotClient = telegramBotClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string hook = string.Format(_options.Url, "api/audio/update");
            await _telegramBotClient.SetWebhookAsync(hook, 
                allowedUpdates: UpdateType.Message.One(), cancellationToken: cancellationToken);
            _logger.LogInformation($"Web Hook is installed");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted service stopping");
            return Task.CompletedTask;
        }

    }
}
