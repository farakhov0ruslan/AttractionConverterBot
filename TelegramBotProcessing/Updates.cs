using AttractionProcessing;
using FileProcessing;
using LoggingProcessing;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotProcessing;

/// <summary>
/// Static class Updates containing methods to handle various types of updates (text messages, documents) received from the Telegram bot.
/// </summary>
public static class Updates
{
    /// <summary>
    /// Handles the processing of text messages received from the Telegram bot.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="message">The message object containing the text message.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    private static async Task TextMessageHandler(ITelegramBotClient botClient, Message message,
        CancellationToken cancellationToken)
    {
        long chatId = message.Chat.Id;
        switch (message.Text) // Switch конструкция для определения логики бота по его текстовосу сообщению.
        {
            case "/start":
                await BotAnswer.Text(botClient, chatId, cancellationToken, BotAnswer.Start,
                    MenuKeyboards.FormatFileKeyboard);
                if (!UsersData.InfoFsmContext.ContainsKey(chatId)) // Добавляем пользователя в машину состояний.
                {
                    UsersData.InfoFsmContext.Add(chatId, new FsmContext(chatId));
                }
                else
                {
                    UsersData.InfoFsmContext[chatId] = new FsmContext(chatId);
                }

                break;

            case "JSON" or "CSV": // Устаналиваем в машину состояний расширение и направляем пользователя дальше.
                await BotAnswer.Text(botClient, chatId, cancellationToken, "Отправьте файл выбранного формата.",
                    new ReplyKeyboardRemove());
                UsersData.InfoFsmContext[chatId].Expansion = message.Text.ToLower();
                break;

            case "Отсортировать": // Направляем пользователя в меню для сортировки.
                await BotAnswer.Text(botClient, chatId, cancellationToken,
                    "Выберите поле для сортировки и порядок.", MenuKeyboards.SortFieldKeyboard);
                break;

            // Кейс срабатывает когда пользовател ввел сообщение которое содерижит сортировку.
            case var str when (str ?? string.Empty).Contains("Сортировать по имени"):
                await DataConverter.SortTask(botClient, chatId, cancellationToken, str);
                break;

            case "Произвести выборку": // Направляем пользователя в меню для выборки.
                await BotAnswer.Text(botClient, chatId, cancellationToken,
                    "Выберите по каким поля делать выборку.", MenuKeyboards.FilterFieldsKeyboard);
                break;

            // Кейс срабатывает когда пользовател ввел сообщение которое содерижит выборку.
            case var str when (str ?? string.Empty).Contains("Выборка"):
                await DataConverter.FilterFieldChoiceTask(botClient, chatId, cancellationToken, str);
                break;

            // Кейс срабатывает когда пользовател ввел сообщение которое содерижит значение для выборки.
            case var str when (str ?? string.Empty).Contains("Значение"):
                await DataConverter.FilterValueChoiceTask(botClient, chatId, cancellationToken, str);
                break;

            // Кейс для скачивания файла срабатывает когда пользовател ввел сообщение которое содерижит скачать.
            case var str when (str ?? string.Empty).Contains("Скачать"):
                await DownoloadFileHandler(botClient, chatId, str!, cancellationToken);
                break;

            case var str when (str ?? string.Empty).Contains("Загрузить новый файл"):
                await BotAnswer.Text(botClient, chatId, cancellationToken,
                    "Выберите расишрение, для загрузки файла.", MenuKeyboards.FormatFileKeyboard);
                UsersData.InfoFsmContext[chatId] = new FsmContext(chatId);
                break;

            default: // Кейс для неизвестных сообщений.
                await BotAnswer.Text(botClient, chatId, cancellationToken, "Неизвестная команда! Начните сначала.",
                    MenuKeyboards.FormatFileKeyboard);
                UsersData.InfoFsmContext[chatId] = new FsmContext(chatId);
                break;
        }
    }


    /// <summary>
    /// Handles the downloading and sending of files in the requested format (CSV or JSON).
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="chatId">The ID of the chat where the request originated.</param>
    /// <param name="text">The user's input text indicating the requested file format.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    private static async Task DownoloadFileHandler(ITelegramBotClient botClient, long chatId, string text,
        CancellationToken cancellationToken)
    {
        if (!UsersData.InfoFsmContext[chatId].StepEnterAttractions) // Проверка на ввод файла.
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "Вы ещё не отправили файл, для начала выберите расширение.", MenuKeyboards.FormatFileKeyboard);
            UsersData.InfoFsmContext[chatId] = new FsmContext(chatId);
            return;
        }

        // Проверка, что пользователь ввёл корректное сообщение для скачивания.
        if (!Array.Exists(new[] { "Скачать файл", "Скачать в CSV", "Скачать в JSON" }, s => s == text))
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "Введён неверная команда для скачивания, повторите ввод.", MenuKeyboards.FileChoiceKeyboard);
            return;
        }

        if (text == "Скачать файл") // Направляем пользователя в меню для выбора формата скачивания.
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "Выберете в каком формате вы хотите скачать данные.", MenuKeyboards.DownoloadFileKeyboard);
            return;
        }

        List<Attraction> dataDownoload = UsersData.InfoFsmContext[chatId].Attractions; // Данные для загрузки.
        try
        {
            if (text.Contains("JSON"))
            {
                UsersData.InfoFsmContext[chatId].DownoloadFileExpansion = ".json";
                await BotAnswer.Document(botClient, chatId, JSONProcessing.Write(dataDownoload));
            }
            else
            {
                UsersData.InfoFsmContext[chatId].DownoloadFileExpansion = ".csv";
                await BotAnswer.Document(botClient, chatId, CSVProcessing.Write(dataDownoload));
            }
        }
        catch (Exception e)
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "При скачивании данных произошла ошибка :(, отправьте другой файл.", MenuKeyboards.FormatFileKeyboard);
            UsersData.InfoFsmContext[chatId] = new FsmContext(chatId);
            Log.Error($"При скачивании данных в чате id:{chatId} произошла ошибка", e);
            return;
        }

        await BotAnswer.Text(botClient, chatId, cancellationToken,
            "Данные успешно отправлены!", MenuKeyboards.FileChoiceKeyboard);

        string filename = UsersData.InfoFsmContext[chatId].FileName +
                          UsersData.InfoFsmContext[chatId].DownoloadFileExpansion.ToLower();
        Log.Info($"[Отправлен документ name:{filename}; в чате: {chatId}]");
    }


    /// <summary>
    /// Handles the processing of document (file) messages received from the Telegram bot.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="document">The document object containing the file information.</param>
    /// <param name="chatId">The ID of the chat where the document was received.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    private static async Task DocumentMessageHandler(ITelegramBotClient botClient, Document document, long chatId,
        CancellationToken cancellationToken)
    {
        string fileId = document.FileId;
        var fileInfo = await botClient.GetFileAsync(fileId, cancellationToken: cancellationToken);
        string filePath = fileInfo.FilePath ?? string.Empty;
        string expansion = UsersData.InfoFsmContext[chatId].Expansion;
        string fileName = document.FileName ?? string.Empty;
        if (!fileName.Contains(expansion)) // Проверка, что пользователь отправил файл с нужным расширением.
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "Вы скинули файл, с неверным расширением, повторите попытку.", new ReplyKeyboardRemove());
            return;
        }

        UsersData.InfoFsmContext[chatId].FileName = fileName.Replace($".{expansion}", "");

        var stream = new MemoryStream(); // Открываем поток памяти, чтобы потом отправить его пользователю как файл. 
        // Загружаем необходимый нам файл.
        await botClient.DownloadFileAsync(filePath, stream, cancellationToken);
        stream.Position = 0;
        List<Attraction>? readData = null;
        try
        {
            switch (UsersData.InfoFsmContext[chatId].Expansion)
            {
                case "csv":
                    readData = CSVProcessing.Read(stream);
                    break;
                case "json":
                    readData = JSONProcessing.Read(stream);
                    break;
            }
        }
        catch (Exception e)
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "При чтении файла произошла ошибка, отправьте другой файл.", MenuKeyboards.FormatFileKeyboard);
            UsersData.InfoFsmContext[chatId] = new FsmContext(chatId);
            Log.Error($"При чтении данных в чате id:{chatId} произошла ошибка", e);
            return;
        }


        if (readData == null) // Проверка на корректность введённых данных.
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "Вы ввели файл с некорректными данными, отправьте другой файл.",
                new ReplyKeyboardRemove());
            return;
        }

        stream.Close(); // Закрываем поток памяти.
        UsersData.InfoFsmContext[chatId].Attractions = readData; // Сохраняем в данные пользователя считанные данные.
        UsersData.InfoFsmContext[chatId].StepEnterAttractions = true;
        await BotAnswer.Text(botClient, chatId, cancellationToken, "Отлично! Теперь выбери следующий шаг.",
            MenuKeyboards.FileChoiceKeyboard);
        Log.Info($"Скачан документ, id:{document.FileId} name:{document.FileName}; в чате: {chatId}");
    }

    /// <summary>
    /// Handles the processing of updates received from the Telegram bot.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="update">The update object containing the message or document information.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var message = update.Message;

        if (message == null)
        {
            return;
        }

        var chatId = message.Chat.Id;

        if (!UsersData.InfoFsmContext.ContainsKey(chatId)) // Проверка что пользователь с существует машине состояний;
        {
            UsersData.InfoFsmContext.Add(chatId, new FsmContext(chatId));
        }

        if (message.Text != null) // Напрвляем пользователя в логику для текстовых сообщений (кнопок).
        {
            await TextMessageHandler(botClient, message, cancellationToken);
            Log.Info($"Получено сообщение: {message.Text}, в чате :{chatId}");
        }

        if (message.Document != null) // Напрвляем пользователя в логику для файлов .
        {
            if (UsersData.InfoFsmContext[chatId].Expansion is not ("csv" or "json"))
            {
                await BotAnswer.Text(botClient, chatId, cancellationToken, "Расширение файла, ещё не выбрано!",
                    MenuKeyboards.FormatFileKeyboard);
                return;
            }

            Log.Info($"Получен документ, id:{message.Document.FileId}" +
                     $" name:{message.Document.FileName}; в чате: {chatId}");
            try
            {
                await DocumentMessageHandler(botClient, message.Document, chatId, cancellationToken);
            }
            catch (Exception e)
            {
                Log.Error($"Ошибка при получении файла в чате: {chatId}", e);
                await BotAnswer.Text(botClient, chatId, cancellationToken, $"Произошла ошибка {e.Message}" + 
                $" при получении файла, отправьте другой {UsersData.InfoFsmContext[chatId].Expansion} файл.",
                    new ReplyKeyboardRemove());
            }
        }
    }
}