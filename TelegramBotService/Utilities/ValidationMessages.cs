namespace TelegramBotService.Utilities
{
    public static class ValidationMessages
    {
        public static string FileSizeExceedLimitMessage =
            "Извините, размер файла превышает ограничение в 50 Мегабайт для ботов в Telegram. Попробуйте другую запись";
        public static string Mp4DoesNotExistsMessage =
            "Извините, не удаётся скачать трэк. Попробуйте другую запись";
        public static string SomeErrorMessage = Mp4DoesNotExistsMessage;
        public static string InvalidLinkMessage =
            "Ссылка невалидна. Введите ссылку на видео Youtube";
        public static string StartCommandReplyMessage = "Пожалуйста, введите ссылку на видео Youtube";
    }
}
