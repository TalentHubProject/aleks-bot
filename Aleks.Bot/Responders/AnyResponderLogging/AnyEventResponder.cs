// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace RemoraDiscordBot.Core.Responders.AnyResponderLogging;

/// <summary>
///     The responder that logs any event received.
/// </summary>
public sealed class AnyEventResponder
    : IResponder<IGatewayEvent>
{
    private readonly ILogger<AnyEventResponder> _logger;

    public AnyEventResponder(ILogger<AnyEventResponder> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Responds to an event.
    /// </summary>
    /// <param name="gatewayEvent">The event.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Result" /> representing the result of the operation.</returns>
    public Task<Result> RespondAsync(IGatewayEvent gatewayEvent, CancellationToken ct = new())
    {
        _logger.LogInformation("Received event {EventName}", gatewayEvent.GetType().Name);

        return Task.FromResult(Result.FromSuccess());
    }
}