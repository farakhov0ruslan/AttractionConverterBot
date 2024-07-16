using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using AttractionProcessing;

namespace FileProcessing;

/// <summary>
/// Provides methods for reading and writing attraction data in JSON format.
/// </summary>
public class JSONProcessing
{
    
    /// <summary>
    /// Reads attraction data from a stream in JSON format.
    /// </summary>
    /// <param name="stream">The input stream containing JSON data.</param>
    /// <returns>A list of attractions read from the JSON data, or null if deserialization fails.</returns>
    public static List<Attraction>? Read(Stream stream)
    {
        var attractions = JsonSerializer.Deserialize<List<Attraction>>(stream);
        return attractions;
    }


    /// <summary>
    /// Writes a list of attractions to a stream in JSON format.
    /// </summary>
    /// <param name="attractions">The list of attractions to write.</param>
    /// <returns>A stream containing the JSON data.</returns>
    /// <remarks>
    /// The returned stream must be closed by the caller after use.
    /// The JSON data is written with indentation and relaxed JSON escaping.
    /// </remarks>
    public static Stream Write(List<Attraction> attractions)
    {
        JsonSerializerOptions options = new JsonSerializerOptions // Устанавливаем настойки для json сериализации.
            { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        string json = JsonSerializer.Serialize(attractions, options);
        var stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

        // Записываем json строку в поток памяти.
        writer.Write(json);
        /* Выгружаем буфер из потока, пока не закрываем, т.к. нам еще нужен поток памяти.
        После закрытия потока памяти поток для записи закроется сам. */
        writer.Flush();
        // Устанавливаю позицию в потоке памяти на нулевую, чтобы потом считывалось корректно.
        stream.Position = 0;

        return stream;
    }
}