using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest.Core;

namespace Aleks.Plugins.StaticStarboard.Services;

/// <inheritdoc cref="IStaticStarboardService" />
public class StaticStarboardDiscordService(
    IDiscordRestChannelAPI channelApi)
    : IStaticStarboardService
{
    /// <summary>
    ///     This property is used to get the star emoji.
    /// </summary>
    public const string StarEmoji = "⭐";

    /// <inheritdoc />
    public IReadOnlySet<Snowflake> StarboardSupportedChannels { get; } = new HashSet<Snowflake>
    {
        new(1069912260643934219),

        // Preview (#création - Zone de Test)
        new(1195408137608495245),

        // Preview (#photographie - Zone de Test)
        new(1195408308304105512),
    };

    /// <inheritdoc />
    public uint MinimumStarsNeeded { get; } = 1;

    /// <inheritdoc />
    public bool IsStarboardSupportedChannel(Snowflake channelId)
    {
        return StarboardSupportedChannels.Contains(channelId);
    }

    /// <inheritdoc />
    public async Task<bool> HasEnoughStars(Snowflake messageId, Snowflake channelId)
    {
        var message = await channelApi.GetChannelMessageAsync(channelId, messageId);

        if (!message.IsSuccess)
        {
            return false;
        }

        var messageEntity = message.Entity;
        var reactions = messageEntity.Reactions;

        var starsCount = 1;

        if (reactions.HasValue)
        {
            starsCount += reactions.Value.Count(reaction => reaction.Emoji.Name == StarEmoji);
        }

        return starsCount >= MinimumStarsNeeded;
    }
}