// // Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// // Licensed under the GNU General Public License v3.0.
// // See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Feedback.Messages;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using RemoraDiscordBot.Plugins.WelcomerFeedbackExperience.Services;

namespace RemoraDiscordBot.Plugins.WelcomerFeedbackExperience.Responders;

public sealed class WelcomerFeedbackMessageResponder
    : IResponder<IMessageCreate>
{
    private readonly FeedbackService _feedbackService;
    private readonly ILogger<WelcomerFeedbackMessageResponder> _logger;
    private readonly IWelcomerFeedbackService _welcomerFeedbackService;

    public WelcomerFeedbackMessageResponder(
        ILogger<WelcomerFeedbackMessageResponder> logger,
        IWelcomerFeedbackService welcomerFeedbackService,
        FeedbackService feedbackService)
    {
        _logger = logger;
        _welcomerFeedbackService = welcomerFeedbackService;
        _feedbackService = feedbackService;
    }

    public async Task<Result> RespondAsync(IMessageCreate gatewayEvent, CancellationToken ct = default)
    {
        if (!gatewayEvent.ReferencedMessage.HasValue)
        {
            return Result.FromError(new InvalidOperationError("No message referenced."));
        }

        var referencedMessage = gatewayEvent.ReferencedMessage.Value;
        
        if (!IsWelcomeMessage(referencedMessage))
        {
            return await Task.FromResult(Result.FromSuccess());
        }

        if (referencedMessage?.Author.ID == null)
        {
            return Result.FromError(new InvalidOperationError($"Referenced message ({referencedMessage.Content}) or author ID is null."));
        }

        if (!gatewayEvent.GuildID.HasValue)
        {
            return Result.FromError(new InvalidOperationError("No guild ID provided."));
        }

        var guildId = gatewayEvent.GuildID;

        _welcomerFeedbackService.AddUser(
            referencedMessage.Author.ID,
            gatewayEvent.Author.ID,
            guildId.Value,
            referencedMessage.ChannelID,
            referencedMessage.Timestamp.DateTime);

        await _feedbackService.SendContextualAsync(
            "Merci de lui avoir souhaité la bienvenue, vous aidez à batir une communauté plus accueillante ! Et vous méritez ces quelques points d'expériences :) <3",
            options: new FeedbackMessageOptions
            {
                MessageFlags = MessageFlags.Ephemeral
            }, ct: ct);

        return Result.FromSuccess();
    }

    private bool IsWelcomeMessage(IMessage message)
    {
        return _welcomerFeedbackService.IsWelcomeMessage(message.ID);
    }
}