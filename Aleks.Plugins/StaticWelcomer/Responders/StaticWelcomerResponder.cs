using Aleks.Business.Colors;
using Aleks.Business.Extensions;
using Microsoft.Extensions.Configuration;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Gateway.Responders;
using Remora.Rest.Core;
using Remora.Results;

namespace Aleks.Plugins.StaticWelcomer.Responders;

/// <summary>
///     Static welcomer responder.
/// </summary>
public class StaticWelcomerResponder(
    IDiscordRestChannelAPI channelApi,
    IConfiguration configuration)
    : IResponder<IGuildMemberAdd>
{
    /// <inheritdoc />
    public async Task<Result> RespondAsync(IGuildMemberAdd gatewayEvent, CancellationToken ct = default)
    {
        var channelId = configuration["WelcomerChannelId"]
                        ?? throw new InvalidOperationException("WelcomerChannelId is not configured.");

        var embed = new Embed
        {
            Colour = DiscordTransparentColor.Value,
            Description = $"{gatewayEvent.User.Value.ID.ToMention()} vient de rejoindre le serveur !",
        };

        var channelIdSnowflake = Snowflake.TryParse(channelId, out var snowflake)
            ? snowflake
            : throw new InvalidOperationException("WelcomerChannelId is not a valid snowflake.");

        return (Result)await channelApi.CreateMessageAsync(channelIdSnowflake!.Value, embeds: new[] { embed }, ct: ct);
    }
}