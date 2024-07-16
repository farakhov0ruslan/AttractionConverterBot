namespace TelegramBotProcessing;

/// <summary>
/// Static class UsersData containing a dictionary to store user-specific finite state machine (FSM) contexts.
/// </summary>
public static class UsersData
{
    /// <summary>
    /// A dictionary that maps chat IDs to FsmContext objects representing the state of each user'
    /// s interaction with the bot.
    /// </summary>
    public static Dictionary<long, FsmContext> InfoFsmContext { get; set; } = new();
}