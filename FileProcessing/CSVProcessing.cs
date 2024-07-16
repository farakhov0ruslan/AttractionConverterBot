using System.ComponentModel;
using System.Text;
using AttractionProcessing;

namespace FileProcessing;

/// <summary>
/// Provides methods for reading and writing data in CSV format.
/// </summary>
public class CSVProcessing
{
    /// <summary>
    /// Reads attraction data from a stream in CSV format.
    /// </summary>
    /// <param name="stream">The input stream containing CSV data.</param>
    /// <returns>A list of attractions read from the CSV data, or null if format is incorrect.</returns>
    public static List<Attraction>? Read(Stream stream)
    {
        char delimiter = ';';
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
            var headerEngLine = reader.ReadLine(); // Читаем строку заголовков
            var headerRusLine = reader.ReadLine();

            if (!(AttractionFormatChecker.FormatCorrectEngHeader(headerEngLine) &&
                  AttractionFormatChecker.FormatCorrectRusHeader(headerRusLine)))
            {
                return null; // Проверка что файл соотвествует шаблону заголовков.
            }

            var properties = typeof(Attraction).GetProperties();
            List<Attraction> readData = new List<Attraction>();
            while (reader.ReadLine() is { } line) // Cчитывние файла.
            {
                if (line == string.Empty)
                {
                    continue;
                }

                string?[] fields = line.Split(delimiter);
                Attraction attraction = new();

                for (int j = 0; j < fields.Length; j++)
                {
                    if (j < properties.Length)
                    {
                        var property = properties[j];
                        object? value = ConvertValue(fields[j], property.PropertyType);
                        property.SetValue(attraction, value);
                    }
                }

                readData.Add(attraction);
            }

            return readData;
        }
    }


    /// <summary>
    /// Converts a string value to the specified type.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="propertyType">The type to convert the value to.</param>
    /// <returns>The converted value, or null if the conversion fails.</returns>
    private static object? ConvertValue(string? value, Type propertyType)
    {
        if (propertyType == typeof(string))
            return value;

        var converter = TypeDescriptor.GetConverter(propertyType);
        return converter.ConvertFromString(value ?? string.Empty);
    }


    /// <summary>
    /// Writes a list of attractions to a stream in CSV format.
    /// </summary>
    /// <param name="attractions">The list of attractions to write.</param>
    /// <returns>A stream containing the CSV data.</returns>
    /// <remarks>
    /// The returned stream must be closed by the caller after use.
    /// </remarks>
    public static Stream Write(List<Attraction> attractions)
    {
        MemoryStream memoryStream = new MemoryStream(); // Создаем поток памяти для записи в него.
        StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
        streamWriter.WriteLine(AttractionFormatChecker.HeaderEng); // Записываем заголовки.
        streamWriter.WriteLine(AttractionFormatChecker.HeaderRus);
        foreach (var att in attractions)
        {
            string[] fields = // Собираем строку строку для записи из значений обьекта.
            {
                att.Name, att.Photo, att.AdmArea, att.District, att.Location,
                att.RegistrationNumber, att.State, att.LocationType, att.GlobalId,
                att.GeoDataCenter, att.GeoArea,
            };
            streamWriter.WriteLine(string.Join(';', CreateCsvStrings(fields)) + ';');
        }


        /* Выгружаем буфер из потока, пока не закрываем, т.к. нам еще нужен поток памяти.
        После закрытия потока памяти поток для записи закроется сам. */
        streamWriter.Flush();
        memoryStream.Position = 0;
        return memoryStream;
    }


    /// <summary>
    /// Creates an array of CSV-formatted strings from an array of strings.
    /// </summary>
    /// <param name="fields">The array of strings to format.</param>
    /// <returns>An array of CSV-formatted strings.</returns>
    private static string[] CreateCsvStrings(string[] fields)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i] = fields[i] == "\"\"" ? string.Empty : $"\"{fields[i]}\"";
        }

        return fields;
    }
}