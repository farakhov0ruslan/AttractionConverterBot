using IOConsoleProcessing;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using LoggingProcessing;

namespace _3_Farahov_CHW_3_3;

class Program
{
    static async Task Main()
    {
        bool flagExitProram = false; // Переменная для закрытия бота.

        var botClient = new TelegramBotClient(Data.Token);

        using CancellationTokenSource cts = new();

        var me = await botClient.GetMeAsync(cancellationToken: cts.Token);
        

        // Логирование.
        Log.Info($"Бот запущен @{me.Username}");
        IOController.Write("Для прекращения работы введите в консоль ", ConsoleColor.Cyan);
        IOController.WriteLine("stop", ConsoleColor.Red);

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        try
        {
            botClient.StartReceiving(
                updateHandler: TelegramBotProcessing.Updates.HandleUpdateAsync,
                pollingErrorHandler: TelegramBotProcessing.Errors.HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        // Запуск бота.

        while (!flagExitProram)
        {
            // Закрыть бота можно только если ввести в консоль stop.
            if ((IOController.ReadLine() ?? "").ToLower() == "stop")
            {
                flagExitProram = true;
                // Закрываем поток записи логировани.
                Log.LogWriterClose();
                // Посылаем запрос на закрытие бота.
                cts.Cancel();
            }
        }
    }
}