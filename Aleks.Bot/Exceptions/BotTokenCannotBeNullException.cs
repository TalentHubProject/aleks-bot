using System.Runtime.Serialization;

namespace Aleks.Bot.Exceptions;

/// <summary>
///     The exception that is thrown when the bot token is null.
/// </summary>
[Serializable]
public class BotTokenCannotBeNullException
    : Exception
{
    public BotTokenCannotBeNullException()
        : base("The bot token cannot be null, you probably missed to set it in the configuration file.")
    {
    }

    protected BotTokenCannotBeNullException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public BotTokenCannotBeNullException(string? message)
        : base(message)
    {
    }

    public BotTokenCannotBeNullException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}