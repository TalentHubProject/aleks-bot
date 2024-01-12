using Aleks.Business.Colors;
using Aleks.Plugins.StaticStarboard.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Remora.Discord.API;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Gateway.Responders;
using Remora.Rest.Core;
using Remora.Results;

namespace Aleks.Plugins.StaticStarboard.Responders;

/// <summary>
///     This responder is used to handle the event when a message gets enough stars to be posted to the starboard.
/// </summary>
public class StaticStarboardOnEnoughStarsGotResponder(
    IStaticStarboardService staticStarboardService,
    IConfiguration configuration,
    ILogger<StaticStarboardOnEnoughStarsGotResponder> logger,
    IDiscordRestChannelAPI channelApi)
    : IResponder<IMessageReactionAdd>
{
    /// <inheritdoc />
    public async Task<Result> RespondAsync(IMessageReactionAdd gatewayEvent, CancellationToken ct = default)
    {
        logger.LogInformation(
            "Message {MessageId} in channel {ChannelId} got a reaction.",
            gatewayEvent.MessageID,
            gatewayEvent.ChannelID);

        if (!gatewayEvent.Emoji.Name.Equals(StaticStarboardDiscordService.StarEmoji))
        {
            logger.LogInformation(
                "Emoji {EmojiName} is not the star emoji.",
                gatewayEvent.Emoji.Name);

            return Result.FromSuccess();
        }

        if (!staticStarboardService.IsStarboardSupportedChannel(gatewayEvent.ChannelID))
        {
            logger.LogInformation(
                "Channel {ChannelId} is not a supported channel for the starboard.",
                gatewayEvent.ChannelID);

            return Result.FromSuccess();
        }

        if (!await staticStarboardService.HasEnoughStars(gatewayEvent.MessageID, gatewayEvent.ChannelID))
        {
            logger.LogInformation(
                "Message {MessageId} in channel {ChannelId} does not have enough stars.",
                gatewayEvent.MessageID,
                gatewayEvent.ChannelID);

            return Result.FromSuccess();
        }

        if (!ulong.TryParse(configuration["StarboardChannelId"], out var starboardChannelId))
        {
            logger.LogError("StarboardChannelId configuration is missing or invalid.");

            return Result.FromError(
                new InvalidOperationError("StarboardChannelId configuration is missing or invalid."));
        }

        var originalMessageResult =
            await channelApi.GetChannelMessageAsync(gatewayEvent.ChannelID, gatewayEvent.MessageID, ct);

        if (!originalMessageResult.IsSuccess)
        {
            logger.LogError("Failed to retrieve the original message.");

            return Result.FromError(originalMessageResult.Error);
        }

        if (originalMessageResult.Entity.Attachments.Count == 0)
        {
            logger.LogInformation(
                "Message {MessageId} in channel {ChannelId} does not have any attachments.",
                gatewayEvent.MessageID,
                gatewayEvent.ChannelID);

            return Result.FromSuccess();
        }

        var originalMessage = originalMessageResult.Entity;
        var avatarUrl = CDN.GetUserAvatarUrl(originalMessage.Author);

        if (!avatarUrl.IsSuccess)
        {
            logger.LogError("Failed to retrieve the avatar URL.");

            return Result.FromError(avatarUrl.Error);
        }

        var embed = new Embed
        {
            Author = new EmbedAuthor(originalMessage.Author.Username, IconUrl: avatarUrl.Entity.ToString()),
            Description = $"[Voir la création](" +
                          $"https://discord.com/channels/{gatewayEvent.GuildID}/{gatewayEvent.ChannelID}/{gatewayEvent.MessageID})",
            Image = new EmbedImage(originalMessage.Attachments.FirstOrDefault()?.Url ?? string.Empty),
            Colour = DiscordTransparentColor.Value,
        };

        return (Result)await channelApi.CreateMessageAsync(
            new Snowflake(starboardChannelId),
            string.Empty,
            embeds: new[] { embed },
            ct: ct);
    }
}