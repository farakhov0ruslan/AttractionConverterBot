namespace AttractionProcessing;

/// <summary>
/// A static class for checking the format of attraction data headers.
/// </summary>
public static class AttractionFormatChecker
{
    /// <summary>
    /// A string containing the header for attraction data in English, with fields separated by semicolons.
    /// </summary>
    public static string HeaderEng = string.Join(';', new string[]
    {
        "\"Name\"", "\"Photo\"", "\"AdmArea\"", "\"District\"", "\"Location\"",
        "\"RegistrationNumber\"", "\"State\"", "\"LocationType\"", "\"global_id\"",
        "\"geodata_center\"", "\"geoarea\"", ""
    });

    /// <summary>
    /// A string containing the header for attraction data in Russian, with fields separated by semicolons.
    /// </summary>
    public static string HeaderRus = string.Join(';', new string[]
    {
        "\"Название объекта\"", "\"Фотография\"", "\"Административный округ по адресу\"", "\"Район\"",
        "\"Месторасположение\"",
        "\"Государственный регистрационный знак\"", "\"Состояние регистрации\"", "\"Тип места расположения\"",
        "\"global_id\"",
        "\"geodata_center\"", "\"geoarea\"", ""
    });

    /// <summary>
    /// Checks if the provided header string matches the English header format.
    /// </summary>
    /// <param name="header">The header string to check.</param>
    /// <returns>True if the header matches the English format, false otherwise.</returns>
    public static bool FormatCorrectEngHeader(string? header)
    {
        return header == HeaderEng;
    }

    /// <summary>
    /// Checks if the provided header string matches the Russian header format.
    /// </summary>
    /// <param name="header">The header string to check.</param>
    /// <returns>True if the header matches the Russian format, false otherwise.</returns>
    public static bool FormatCorrectRusHeader(string? header)
    {
        return header == HeaderRus;
    }
}