using IOConsoleProcessing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LoggingProcessing;

/// <summary>
/// Static class Log providing methods for logging information messages and errors.
/// </summary>
public static class Log
{
    private static StreamWriter _logWriter = new(GetLogsPath(), true);
    private static object _locker = new();

    /// <summary>
    /// Writes an information message to the log stream.
    /// </summary>
    /// <param name="message">The information message to log.</param>
    public static void Info(string message)
    {
        // Создаём обьект лога информации.
        LogEntry<string> logEntry = new LogEntry<string>(
            LogLevel.Information,
            "Info",
            0,
            message,
            null, // Передаем объект Exception
            (state, _) => // Устанавливаем формат для логирования информации.
                $"[{DateTime.Now}] [{LogLevel.Information.ToString()}] [{state}]"
        );

        Write(logEntry);
    }


    /// <summary>
    /// Writes an error message and the corresponding exception to the log stream.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    /// <param name="ex">The exception object associated with the error.</param>
    public static void Error(string message, Exception ex)
    {
        // Создаём обьект лога ошибки.
        LogEntry<string> logEntry = new LogEntry<string>(
            LogLevel.Error,
            "Error",
            0,
            message,
            ex, // Передаем объект Exception
            (state, exception) => // Устанавливаем формат для логирования ошибки.
                $"[{DateTime.Now}] [{LogLevel.Error.ToString()}] [{state}] [{exception?.Message}]"
        );


        Write(logEntry);
    }


    /// <summary>
    /// Writes a log message to the log stream.
    /// </summary>
    /// <typeparam name="TState">The type of the state passed to the log message formatter.</typeparam>
    /// <param name="logEntry">The LogEntry object containing log information.</param>
    private static void Write<TState>(in LogEntry<TState> logEntry)
    {
        // Из обьекта лога собираем сообщение по шаблону.
        string message = logEntry.Formatter(logEntry.State, logEntry.Exception);

        switch (logEntry.LogLevel) // Дублирование логов в консоль.
        {
            case LogLevel.Information:
                IOController.WriteLine(message, ConsoleColor.Cyan);
                break;

            case LogLevel.Error:
                IOController.WriteLine(message, ConsoleColor.Red);
                break;
        }

        lock (_locker) // Проверяем что поток свободен, чтобы исключить запись 2 сообщений друг на друга.
        {
            _logWriter.WriteLineAsync(message);
            _logWriter.Flush();
        }
    }

    /// <summary>
    /// Closes the stream writer for the log file.
    /// </summary>
    public static void LogWriterClose()
    {
        lock (_locker) // Проверяем что поток свободен, чтобы исключить закрытие потока во время записи.
        {
            _logWriter.Close();
        }
    }


    /// <summary>
    /// Method for get path to directory var for save logs file.
    /// </summary>
    /// <returns></returns>
    private static string GetLogsPath()
    {
        return Path.Combine("var", "logs.txt");
    }
}