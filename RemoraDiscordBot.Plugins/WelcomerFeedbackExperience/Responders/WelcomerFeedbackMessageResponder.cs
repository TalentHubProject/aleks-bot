// // Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// // Licensed under the GNU General Public License v3.0.
// // See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using RemoraDiscordBot.Plugins.WelcomerFeedbackExperience.Services;

namespace RemoraDiscordBot.Plugins.WelcomerFeedbackExperience.Responders;

public sealed class WelcomerFeedbackMessageResponder
    : IResponder<IMessageCreate>
{
    private readonly ILogger<WelcomerFeedbackMessageResponder> _logger;
    private readonly IWelcomerFeedbackService _welcomerFeedbackService;

    public WelcomerFeedbackMessageResponder(
        ILogger<WelcomerFeedbackMessageResponder> logger,
        IWelcomerFeedbackService welcomerFeedbackService)
    {
        _logger = logger;
        _welcomerFeedbackService = welcomerFeedbackService;
    }

    public async Task<Result> RespondAsync(IMessageCreate gatewayEvent, CancellationToken ct = default)
    {
        if (!gatewayEvent.ReferencedMessage.HasValue)
        {
            return Result.FromError(new InvalidOperationError("No message referenced."));
        }

        var referencedMessage = gatewayEvent.ReferencedMessage.Value;


        if (referencedMessage?.Author.ID == null)
        {
            return Result.FromError(new InvalidOperationError("Referenced message or author ID is null."));
        }

        if (!gatewayEvent.GuildID.HasValue)
        {
            return Result.FromError(new InvalidOperationError("No guild ID provided."));
        }

        var guildId = gatewayEvent.GuildID;

        if (!IsWelcomeMessage(referencedMessage))
        {
            return Result.FromError(new InvalidOperationError("Message is not a welcome message."));
        }

        _welcomerFeedbackService.AddUser(
            referencedMessage.Author.ID,
            referencedMessage.Author.ID,
            guildId.Value,
            referencedMessage.ChannelID,
            referencedMessage.Timestamp.DateTime);

        return Result.FromSuccess();
    }

    private bool IsWelcomeMessage(IMessage message)
    {
        return _welcomerFeedbackService.IsWelcomeMessage(message.ID);
    }
}