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
    public async Task<Result> RespondAsync(IMessageReactionAdd gatewayEvent, CancellationToken ct = default)
    {
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
            var message = await channelApi.GetChannelMessageAsync(gatewayEvent.ChannelID, gatewayEvent.MessageID, ct);

            if (!message.IsSuccess)
            {
                return Result.FromError(message.Error);
            }

            var reactions = message.Entity.Reactions;

            if (!reactions.HasValue)
            {
                return Result.FromSuccess();
            }

            return Result.FromSuccess();
        }

        if (!ulong.TryParse(configuration["StarboardChannelId"], out var starboardChannelId))
        {
            return Result.FromError(
                new InvalidOperationError("StarboardChannelId configuration is missing or invalid."));
        }

        var starboardChannelIdSnowflake = new Snowflake(starboardChannelId);

        var originalMessageResult =
            await channelApi.GetChannelMessageAsync(gatewayEvent.ChannelID, gatewayEvent.MessageID, ct);

        if (!originalMessageResult.IsSuccess)
        {
            logger.LogError("Failed to retrieve the original message.");
            return Result.FromError(originalMessageResult.Error);
        }

        if (originalMessageResult.Entity.Attachments.Count == 0)
        {
            return Result.FromSuccess();
        }

        var originalMessage = originalMessageResult.Entity;
        var avatarUrl = CDN.GetUserAvatarUrl(originalMessage.Author);

        if (!avatarUrl.IsSuccess)
        {
            throw new InvalidOperationException("Failed to get the avatar URL.");
        }

        var embed = new Embed
        {
            Author = new EmbedAuthor(originalMessage.Author.Username, IconUrl: avatarUrl.Entity.ToString()),
            Description = $"[Voir la création](" +
                          $"https://discord.com/channels/{gatewayEvent.GuildID}/{gatewayEvent.ChannelID}/{gatewayEvent.MessageID})",
            Image = new EmbedImage(originalMessage.Attachments.FirstOrDefault()?.Url ?? ""),
            Colour = DiscordTransparentColor.Value,
        };

        return (Result)await channelApi.CreateMessageAsync(
            new Snowflake(starboardChannelId),
            string.Empty,
            embeds: new[] { embed },
            ct: ct);
    }
}