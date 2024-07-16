namespace TelegramBotProcessing;

using Telegram.Bot.Types.ReplyMarkups;

/// <summary>
/// Static class MenuKeyboards containing pre-defined and custom reply keyboard markups for the Telegram bot.
/// </summary>
public static class MenuKeyboards
{
    /// <summary>
    /// A reply keyboard markup with buttons for selecting the file format (CSV or JSON).
    /// </summary>
    public static readonly ReplyKeyboardMarkup FormatFileKeyboard = new(new[]
    {
        new KeyboardButton[] { "CSV", "JSON" },
    })
    {
        ResizeKeyboard = true
    };


    /// <summary>
    /// A reply keyboard markup with buttons for various file operations (filtering, sorting, downloading, and uploading).
    /// </summary>
    public static readonly ReplyKeyboardMarkup FileChoiceKeyboard = new(new[]
    {
        new KeyboardButton[] { "Произвести выборку" },
        new KeyboardButton[] { "Отсортировать" },
        new KeyboardButton[] { "Скачать файл" },
        new KeyboardButton[] { "Загрузить новый файл" },
    })
    {
        ResizeKeyboard = true
    };

    /// <summary>
    /// A reply keyboard markup with buttons for sorting attractions by name in ascending or descending order.
    /// </summary>
    public static readonly ReplyKeyboardMarkup SortFieldKeyboard = new(new[]
    {
        new KeyboardButton[] { "Сортировать по имени(Name) в прямом порядке" },
        new KeyboardButton[] { "Сортировать по имени(Name) в обратном порядке" },
    });

    /// <summary>
    /// A reply keyboard markup with buttons for filtering attractions by various fields.
    /// </summary>
    public static readonly ReplyKeyboardMarkup FilterFieldsKeyboard = new(new[]
    {
        new KeyboardButton[] { "Выборка по Name" },
        new KeyboardButton[] { "Выборка по Photo" },
        new KeyboardButton[] { "Выборка по AdmArea" },
        new KeyboardButton[] { "Выборка по District" },
        new KeyboardButton[] { "Выборка по Location" },
        new KeyboardButton[] { "Выборка по RegistrationNumber" },
        new KeyboardButton[] { "Выборка по State" },
        new KeyboardButton[] { "Выборка по LocationType" },
        new KeyboardButton[] { "Выборка по global_id" },
        new KeyboardButton[] { "Выборка по AdmArea и Location" }
    })
    {
        ResizeKeyboard = true
    };


    /// <summary>
    /// Creates a custom reply keyboard markup with buttons containing the provided list of string values.
    /// </summary>
    /// <param name="nameButtons">The list of string values to be used as button texts.</param>
    /// <returns>A ReplyKeyboardMarkup object representing the custom keyboard.</returns>
    public static ReplyKeyboardMarkup CustomKeyboard(List<string> nameButtons)
    {
        KeyboardButton[][] kb = new KeyboardButton[nameButtons.Count][];
        for (int i = 0; i < nameButtons.Count; i++)
        {
            kb[i] = new KeyboardButton[] { nameButtons[i] };
        }

        ReplyKeyboardMarkup replyKeyboardMarkup = new(kb)
        {
            ResizeKeyboard = true
        };
        return replyKeyboardMarkup;
    }

    /// <summary>
    /// A reply keyboard markup with buttons for downloading the file in CSV or JSON format.
    /// </summary>
    public static readonly ReplyKeyboardMarkup DownoloadFileKeyboard = new(new[]
    {
        new KeyboardButton[] { "Скачать в CSV" },
        new KeyboardButton[] { "Скачать в JSON" }
    });
}