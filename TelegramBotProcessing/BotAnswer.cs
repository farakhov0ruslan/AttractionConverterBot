using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotProcessing;

/// <summary>
/// Static class BotAnswer providing methods for sending responses to Telegram bot users.
/// </summary>
public static class BotAnswer
{
    /// <summary>
    /// A constant string representing the start message for the bot.
    /// </summary>
    public const string Start = "Добро пожаловать в бота для работы с данными об атракционах! " +
                                "Для продолжения работы выберите с каким форматом данных работать.";

    /// <summary>
    /// A constant string representing the success message after sorting data.
    /// </summary>
    public const string SuccesSort = "Данные успешно отсортированы. Вы можете скачать файл, или продолжить" +
                                     " работу с изменеёнными данными.";

    /// <summary>
    /// Sends a text message with a reply keyboard markup to the specified chat.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="chatId">The ID of the chat to send the message to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <param name="message">The text message to send.</param>
    /// <param name="replyKeyboardMarkup">The reply keyboard markup to include with the message.</param>
    public static async Task Text(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken,
        string message, ReplyKeyboardMarkup replyKeyboardMarkup)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: message,
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Sends a text message with a remove keyboard markup to the specified chat.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="chatId">The ID of the chat to send the message to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <param name="message">The text message to send.</param>
    /// <param name="replyKeyboardRemove">The remove keyboard markup to include with the message.</param>
    public static async Task Text(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken,
        string message, ReplyKeyboardRemove replyKeyboardRemove)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: message,
            replyMarkup: replyKeyboardRemove,
            cancellationToken: cancellationToken);
    }


    /// <summary>
    /// Sends a document (file) to the specified chat.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="chatId">The ID of the chat to send the document to.</param>
    /// <param name="stream">The stream containing the document data.</param>
    public static async Task Document(ITelegramBotClient botClient, long chatId, Stream stream)
    {
        string filename = UsersData.InfoFsmContext[chatId].FileName +
                          UsersData.InfoFsmContext[chatId].DownoloadFileExpansion.ToLower();
        await botClient.SendDocumentAsync(
            chatId: chatId,
            document: InputFile.FromStream(stream, fileName: filename));
        stream.Close();
    }
}