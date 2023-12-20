// // Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// // Licensed under the GNU General Public License v3.0.
// // See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using Aleks.Business.Extensions;
using Aleks.Plugins.Welcomer.Queries;
using Aleks.Plugins.WelcomerFeedbackExperience.Services;

namespace Aleks.Plugins.WelcomerFeedbackExperience.Responders;

public sealed class WelcomerFeedbackGuildMemberJoinResponder
    : IResponder<IGuildMemberAdd>
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly IWelcomerFeedbackService _welcomerFeedbackService;
    private readonly IMediator _mediator;

    public WelcomerFeedbackGuildMemberJoinResponder(
        IDiscordRestChannelAPI channelApi, 
        IWelcomerFeedbackService welcomerFeedbackService, 
        IMediator mediator)
    {
        _channelApi = channelApi;
        _welcomerFeedbackService = welcomerFeedbackService;
        _mediator = mediator;
    }

    public async Task<Result> RespondAsync(
        IGuildMemberAdd gatewayEvent,
        CancellationToken ct = default)
    {
        var welcomer = await _mediator.Send(new GetsIfGuildAlreadyRegisteredQuery(gatewayEvent.GuildID), ct);

        if (welcomer.WelcomeChannelId is null)
            return Result.FromSuccess();
        

        var feedbackMessage = await _channelApi.CreateMessageAsync(
            welcomer.WelcomeChannelId.Value.ToSnowflake(),
            $"*{gatewayEvent.User.Value.Username} vient de rejoindre le serveur ! Répondez à ce message pour lui souhaiter la bienvenue et gagner de l'expérience.* ✨",
            ct: ct);
        
        if (!feedbackMessage.IsSuccess)
        {
            return Result.FromError(new InvalidOperationError("Message is not a welcome message."));
        }

        _welcomerFeedbackService.AddWelcomeMessageId(feedbackMessage.Entity.ID);

        return Result.FromSuccess();
    }
}