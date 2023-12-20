using System.Text.RegularExpressions;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace Aleks.Plugins.AdvertisementGuard.Responders;

/// <summary>
///     The responder to delete messages containing discord invites.
/// </summary>
/// <param name="channelApi">The injected discord rest channel API.</param>
/// <param name="feedbackService">The injected feedback service.</param>
public partial class UserMessageAdvertisementGuardResponder(
    IDiscordRestChannelAPI channelApi,
    FeedbackService feedbackService)
    : IResponder<IMessageCreate>
{
    private const string DISCORD_INVITE_REGEX =
        @"(https?:\/\/)?(www\.)?(discord\.(gg|io|me|li)|discordapp\.com\/invite)\/.+[a-z]";

    public async Task<Result> RespondAsync(IMessageCreate gatewayEvent, CancellationToken ct = default)
    {
        if (gatewayEvent.Author.IsBot is { HasValue: true, Value: true })
        {
            return Result.FromSuccess();
        }

        if (gatewayEvent.Member.Value.Permissions is { HasValue: true } &&
            gatewayEvent.Member.Value.Permissions.Value.HasPermission(DiscordTextPermission.ManageMessages))
        {
            return Result.FromSuccess();
        }

        var message = gatewayEvent.Content;
        if (string.IsNullOrWhiteSpace(message))
        {
            return Result.FromSuccess();
        }

        var match = MyRegex().Match(message);
        if (!match.Success)
        {
            return Result.FromSuccess();
        }

        await channelApi.DeleteMessageAsync(gatewayEvent.ChannelID, gatewayEvent.ID, ct: ct);

        return (Result)await feedbackService.SendErrorAsync(
            gatewayEvent.ChannelID,
            "Il est interdit d'envoyer des liens d'invitation vers d'autres serveurs Discord.",
            gatewayEvent.Author.ID,
            ct: ct);
    }

    [GeneratedRegex(DISCORD_INVITE_REGEX, RegexOptions.IgnoreCase, "fr-FR")]
    private static partial Regex MyRegex();
}