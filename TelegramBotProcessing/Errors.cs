using LoggingProcessing;
using Telegram.Bot;
using Telegram.Bot.Exceptions;


namespace TelegramBotProcessing;

/// <summary>
/// Class Errors containing methods to handle errors and exceptions in the Telegram bot.
/// </summary>
public class Errors
{
    /// <summary>
    /// Handles errors that occur during the bot's polling process.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A completed Task representing the asynchronous operation.</returns>
    public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        IOConsoleProcessing.IOController.WriteLine(errorMessage, ConsoleColor.Red);
        Log.Error("", exception);
        IOConsoleProcessing.IOController.WriteLine(exception, ConsoleColor.Red);

        return Task.CompletedTask;
    }
}