using System.Collections.Generic;
using Telegram.Bot.Types.Enums;

namespace TelegramBotService.Extensions
{
    internal static class ObjectExtensions
    {
        public static IEnumerable<UpdateType> One(this UpdateType type)
        {
            yield return UpdateType.Message;
        }

    }
}
