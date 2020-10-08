namespace TelegramBotService.Utilities
{
    public static class Constants
    {
        // Regex to check valid URL 
        public const string regexUrl = "((http|https)://)(www.)?[a-zA-Z0-9@:%._\\+~#?&//=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%._\\+~#?&//=]*)";

        public static class ValidationMessages
        {
            public const string FileSizeExceedLimitMessage =
                "Извините, размер файла превышает ограничение в 50 Мегабайт для ботов в Telegram. Попробуйте другую запись";
            public const string Mp4DoesNotExistsMessage =
                "Извините, не удаётся скачать трэк. Попробуйте другую запись";
            public const string SomeErrorMessage = Mp4DoesNotExistsMessage;
            public const string InvalidLinkMessage =
                "Ссылка невалидна. Введите ссылку на видео Youtube";
            public const string StartCommandReplyMessage = "Пожалуйста, введите ссылку на видео Youtube";
        }
    }
}
