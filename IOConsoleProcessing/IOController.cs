namespace IOConsoleProcessing;

/// <summary>
/// A class to use the color console.
/// </summary>
public static class IOController
{
    /// <summary>
    /// Analog of the "Console.Write" method with colored text.
    /// </summary>
    /// <param name="writeObject">object to output</param>
    /// <param name="color">Color which console outbut writeObject</param>
    public static void Write(object writeObject, ConsoleColor color)
    {
        Console.ForegroundColor = color; // Устанавливаю цвет вывода.
        Console.Write(writeObject);
        Console.ResetColor(); // Возвращаю цвет к стандартному.
    }

    /// <summary>
    /// Analog of the "Console.WriteLine" method with colored text.
    /// </summary>
    /// <param name="writeObject">object to output</param>
    /// <param name="color">Color which console outbut writeObject</param>
    public static void WriteLine(object writeObject, ConsoleColor color)
    {
        Console.ForegroundColor = color; // Устанавливаю цвет вывода.
        Console.WriteLine(writeObject);
        Console.ResetColor(); // Возвращаю цвет к стандартному.
    }

    /// <summary>
    /// Analog of the "Console.ReadLine" method with colored text.
    /// </summary>
    public static string? ReadLine()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        string? resultString = Console.ReadLine(); // Устанавливаю цвет вывода.
        Console.ResetColor(); // Возвращаю цвет к стандартному.
        return resultString;
    }

    /// <summary>
    ///A method that prints separators for different stages of the program.
    /// </summary>
    public static void PrintSeparators()
    {
        WriteLine(new string('_', 80), ConsoleColor.Blue);
    }
}