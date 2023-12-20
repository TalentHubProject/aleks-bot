using Remora.Rest.Core;

namespace Aleks.Plugins.StaticStarboard.Services;

/// <summary>
///     This interface is used to define the service that is used to handle the starboard.
/// </summary>
public interface IStaticStarboardService
{
    /// <summary>
    ///     This property is used to get the starboard supported channels snowflakes.
    /// </summary>
    IReadOnlySet<Snowflake> StarboardSupportedChannels { get; }

    /// <summary>
    ///     This property is used to get the minimum amount of stars needed to post a message to the starboard.
    /// </summary>
    uint MinimumStarsNeeded { get; }

    /// <summary>
    ///     This method is used to check if the given channel is a starboard supported channel.
    /// </summary>
    /// <param name="channelId">The id of the channel to check.</param>
    /// <returns>True if the channel is a starboard supported channel, false otherwise.</returns>
    bool IsStarboardSupportedChannel(Snowflake channelId);

    /// <summary>
    ///     This method is used to check if the given message is a starboard message.
    /// </summary>
    /// <param name="messageId">The id of the message to check.</param>
    /// <param name="channelId">The id of the channel to check.</param>
    /// <returns>True if the message is a starboard message, false otherwise.</returns>
    Task<bool> HasEnoughStars(Snowflake messageId, Snowflake channelId);
}