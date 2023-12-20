using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace Aleks.Bot.Responders.AnyResponderLogging;

/// <summary>
///     The responder that logs any event received.
/// </summary>
public sealed class AnyEventResponder(ILogger<AnyEventResponder> logger) : IResponder<IGatewayEvent>
{
    /// <summary>
    ///     Responds to an event.
    /// </summary>
    /// <param name="gatewayEvent">The event.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Result" /> representing the result of the operation.</returns>
    public Task<Result> RespondAsync(IGatewayEvent gatewayEvent, CancellationToken ct = default)
    {
        logger.LogInformation("Received event {EventName}", gatewayEvent.GetType().Name);

        return Task.FromResult(Result.FromSuccess());
    }
}