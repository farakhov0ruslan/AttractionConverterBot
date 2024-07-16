using AttractionProcessing;
using LoggingProcessing;
using Telegram.Bot;

namespace TelegramBotProcessing;

/// <summary>
/// Static class DataConverter providing methods for sorting and filtering attraction data.
/// </summary>
public static class DataConverter
{
    /// <summary>
    /// Sorts the attractions based on the user's choice of ascending or descending order.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="chatId">The ID of the chat where the request originated.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <param name="text">The user's input text indicating the sort order.</param>
    public static async Task SortTask(ITelegramBotClient botClient, long chatId,
        CancellationToken cancellationToken, string? text)
    {
        if (!UsersData.InfoFsmContext[chatId].StepEnterAttractions) // Проверяем что пользователь вводил файл.
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "Вы ещё не ввели файл, повторите ввод", MenuKeyboards.FormatFileKeyboard);
            UsersData.InfoFsmContext[chatId] = new FsmContext(chatId);
            return;
        }

        // Переменная которая отвечает за порядок сортировки.
        bool sortReverse = !(text ?? string.Empty).Contains("прямом");
        try
        {
            UsersData.InfoFsmContext[chatId].Attractions.Sort(
                (x, y) => x.CompareTo(y, sortReverse));
        }
        catch (Exception e)
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "При сортировке возникла ошибка :(, отправьте другой файл.", MenuKeyboards.FormatFileKeyboard);
            UsersData.InfoFsmContext[chatId] = new FsmContext(chatId);
            Log.Error($"При сортировке данных в чате id:{chatId} произошла ошибка", e);
            return;
        }

        await BotAnswer.Text(botClient, chatId, cancellationToken, BotAnswer.SuccesSort,
            MenuKeyboards.FileChoiceKeyboard);
    }

    /// <summary>
    /// Filters the attractions based on the chosen field and value.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="chatId">The ID of the chat where the request originated.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <param name="text">The user's input text indicating the filter value.</param>
    public static async Task FilterValueChoiceTask(ITelegramBotClient botClient, long chatId,
        CancellationToken cancellationToken, string? text)
    {
        if (!UsersData.InfoFsmContext[chatId].StepEnterFilterField) // Проверяем что пользователь вводил поля для выборки.
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken, "Вы еще не ввели поле для выборки!",
                MenuKeyboards.FileChoiceKeyboard);
            return;
        }

        List<Attraction> attractionsFilter;
        try
        {
            // Выборка через Linq метод.
            attractionsFilter = UsersData.InfoFsmContext[chatId].Attractions.Where(x =>
            {
                string valueX = x.GetPropertyValue(UsersData.InfoFsmContext[chatId].FilterFields[0]);
                return valueX == (text ?? string.Empty).Replace("Значение ", string.Empty);
            }).ToList();
        }
        catch (Exception e)
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "При выборке возникла ошибка :(, отправьте другой файл.", MenuKeyboards.FormatFileKeyboard);
            UsersData.InfoFsmContext[chatId] = new FsmContext(chatId);
            Log.Error($"При выборке данных в чате id:{chatId} произошла ошибка", e);
            return;
        }
        // Устанавливаем в данные пользователя отфильтрованные данные.
        UsersData.InfoFsmContext[chatId].Attractions = attractionsFilter;
        // Проверка на количество полей для выборки.
        if (UsersData.InfoFsmContext[chatId].FilterFields.Count > 1)
        {
            List<string> possibleValues = Filter.ValuesField(UsersData.InfoFsmContext[chatId].Attractions,
                UsersData.InfoFsmContext[chatId].FilterFields[1]);
            UsersData.InfoFsmContext[chatId].FilterFields.RemoveAt(0);
            // Проверяем что у пользователя непустые данные.
            if (UsersData.InfoFsmContext[chatId].Attractions.Count == 0)
            {
                await BotAnswer.Text(botClient, chatId, cancellationToken,
                    "Сейчас список аттракицонов пуст, для выборки необходимы какие" +
                    " то значение. Можете загрузить новый файл.", MenuKeyboards.FileChoiceKeyboard);
                return;
            }
            
            // Выводим возможные значения для выборки.
            await BotAnswer.Text(botClient, chatId, cancellationToken, "Выберите значения для выборки",
                MenuKeyboards.CustomKeyboard(possibleValues.ConvertAll(x => "Значение " + x)));
        }
        else // Если полей 0 то выборка завершена.
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "Выборка произведена, теперь можете продолжить работу с отфильтрованными" +
                " данными или скачать файл", MenuKeyboards.FileChoiceKeyboard);
        }
    }

    /// <summary>
    /// Prompts the user to choose a field for filtering the attractions.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="chatId">The ID of the chat where the request originated.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <param name="text">The user's input text indicating the chosen filter field.</param>
    public static async Task FilterFieldChoiceTask(ITelegramBotClient botClient, long chatId,
        CancellationToken cancellationToken, string? text)
    {
        // Проверяем что пользователь ввёл файл.
        if (!UsersData.InfoFsmContext[chatId].StepEnterAttractions)
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "Вы ещё не отправили файл, выберите нужное расширение и отправьте файл затем повторите ввод",
                MenuKeyboards.FormatFileKeyboard);
            return;
        }
        
        
        if (!Array.Exists(new[] // Проверем что пользователь ввел корректное значение для выборки.
            {
                "Выборка по District", "Выборка по AdmArea и Location", "Выборка по LocationType",
                "Выборка по Name", "Выборка по Photo", "Выборка по AdmArea", "Выборка по Location",
                "Выборка по RegistrationNumberr", "Выборка по State", "Выборка по global_id"
            }, s => s == text))
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "Вы ввели некорректное поле для выборки, повторите ввод", MenuKeyboards.FileChoiceKeyboard);
            return;
        }

        string[] textSplit = (text ?? string.Empty).Split(); 
        // Получаем из сообщения пользователь поля для выборки.
        string[] filterFields = textSplit.Length == 3 ? new[] { textSplit[2] } : new[] { textSplit[2], textSplit[4] };
        UsersData.InfoFsmContext[chatId].FilterFields = filterFields.ToList();
        // Устанавливаем в машину состояний, что пользователь ввёл поля для фильтрации.
        UsersData.InfoFsmContext[chatId].StepEnterFilterField = true; 
        List<string> possibleValues =
            Filter.ValuesField(UsersData.InfoFsmContext[chatId].Attractions, textSplit[2]);

        // Проверяем что у пользователя не пустые данные.
        if (UsersData.InfoFsmContext[chatId].Attractions.Count == 0) 
        {
            await BotAnswer.Text(botClient, chatId, cancellationToken,
                "Сейчас список аттракицонов пуст, для выборки необходимы какие" +
                " то значение. Можете загрузить новый файл.", MenuKeyboards.FileChoiceKeyboard);
            return;
        }

        await BotAnswer.Text(botClient, chatId, cancellationToken, "Выберите значения для выборки",
            MenuKeyboards.CustomKeyboard(possibleValues.ConvertAll(x => "Значение " + x)));
    }
}