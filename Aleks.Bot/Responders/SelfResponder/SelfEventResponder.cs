using Aleks.Business.Colors;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Gateway.Events;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace Aleks.Bot.Responders.SelfResponder;

/// <summary>
///     The responder that responds to self mentions.
/// </summary>
public sealed class SelfEventResponder(
    FeedbackService feedbackService,
    IDiscordRestUserAPI discordRestUserApi)
    : IResponder<MessageCreate>
{
    /// <summary>
    ///     Responds to an event.
    /// </summary>
    /// <param name="gatewayEvent">The event.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Result" /> representing the result of the operation.</returns>
    public async Task<Result> RespondAsync(
        MessageCreate gatewayEvent,
        CancellationToken ct = default)
    {
        var message = gatewayEvent.Content;
        var botSnowflake = await discordRestUserApi.GetCurrentUserAsync(ct);

        if (gatewayEvent.Author.IsBot is { Value: true, HasValue: true })
        {
            return Result.FromSuccess();
        }

        if (message.Equals($"<@!{botSnowflake.Entity.ID}>"))
        {
            return (Result)await feedbackService.SendContentAsync(
                gatewayEvent.ChannelID,
                "Hey there! I am Aleks!",
                DiscordTransparentColor.Value,
                ct: ct);
        }

        return Result.FromSuccess();
    }
}