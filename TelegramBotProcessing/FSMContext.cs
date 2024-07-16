using AttractionProcessing;

namespace TelegramBotProcessing;

/// <summary>
/// Class FsmContext represents the context or state of a user in the Telegram bot's finite state machine.
/// </summary>
public class FsmContext
{
    private readonly long _chatId;

    /// <summary>
    /// Gets or sets the name of the file uploaded by the user.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the expansion (file extension) of the uploaded file.
    /// </summary>
    public string Expansion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of fields used for filtering attractions.
    /// </summary>
    public List<string> FilterFields { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the user has entered attractions.
    /// </summary>
    public bool StepEnterAttractions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user has entered a filter field.
    /// </summary>
    public bool StepEnterFilterField { get; set; }

    /// <summary>
    /// Gets or sets the expansion (file extension) for the downloaded file.
    /// </summary>
    public string DownoloadFileExpansion { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets the list of attractions associated with the user's context.
    /// </summary>
    public List<Attraction> Attractions { get; set; } = new();


    /// <summary>
    /// Initializes a new instance of the FsmContext class with the specified chat ID.
    /// </summary>
    /// <param name="chatId">The ID of the chat associated with the context.</param>
    public FsmContext(long chatId)
    {
        // Private field to store the chat ID
        _chatId = chatId;
    }

    /// <summary>
    /// Determines whether the current FsmContext object is equal to another FsmContext object.
    /// </summary>
    /// <param name="other">The other FsmContext object to compare with.</param>
    /// <returns>True if the objects are equal, false otherwise.</returns>
    protected bool Equals(FsmContext other)
    {
        return _chatId == other._chatId;
    }

    /// <summary>
    /// Returns the hash code for the current FsmContext object.
    /// </summary>
    /// <returns>The hash code for the object.</returns>
    public override int GetHashCode()
    {
        return _chatId.GetHashCode();
    }
}