// // Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// // Licensed under the GNU General Public License v3.0.
// // See the LICENSE file in the project root for more information.

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using RemoraDiscordBot.Plugins.WelcomerFeedbackExperience.Services;

namespace RemoraDiscordBot.Plugins.WelcomerFeedbackExperience.Responders;

public sealed class WelcomerFeedbackGuildMemberJoinResponder
    : IResponder<IGuildMemberAdd>
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly IWelcomerFeedbackService _welcomerFeedbackService;

    public WelcomerFeedbackGuildMemberJoinResponder(
        IDiscordRestChannelAPI channelApi, 
        IWelcomerFeedbackService welcomerFeedbackService)
    {
        _channelApi = channelApi;
        _welcomerFeedbackService = welcomerFeedbackService;
    }

    public async Task<Result> RespondAsync(
        IGuildMemberAdd gatewayEvent,
        CancellationToken ct = default)
    {
        var userTag = gatewayEvent.User.HasValue
            ? $"<@{gatewayEvent.User.Value.ID}>"
            : throw new InvalidOperationException("Cannot get user ID from gateway event.");

        var feedbackMessage = await _channelApi.CreateMessageAsync(
            gatewayEvent.GuildID,
            $"*{userTag} vient de rejoindre le serveur ! Répondez à ce message pour lui souhaiter la bienvenue et gagner de l'expérience.*",
            ct: ct);
        
        if (!feedbackMessage.IsSuccess)
        {
            return Result.FromError(new InvalidOperationError("Message is not a welcome message."));
        }

        _welcomerFeedbackService.AddWelcomeMessageId(feedbackMessage.Entity.ID);

        return Result.FromSuccess();
    }
}