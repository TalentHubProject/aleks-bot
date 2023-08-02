// // Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// // Licensed under the GNU General Public License v3.0.
// // See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.Extensions.Logging;
using Remora.Rest.Core;
using RemoraDiscordBot.Plugins.Experience.Commands;

namespace RemoraDiscordBot.Plugins.WelcomerFeedbackExperience.Services;

public sealed record WelcomerFeedbackService
    : IWelcomerFeedbackService
{
    private readonly ILogger<WelcomerFeedbackService> _logger;
    private readonly IMediator _mediator;
    private readonly List<WelcomerFeedbackUser> _welcomerFeedbackUsers = new();
    private readonly List<Snowflake> _welcomeMessageIds = new List<Snowflake>();
    
    public WelcomerFeedbackService(
        ILogger<WelcomerFeedbackService> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    public void AddWelcomeMessageId(Snowflake messageId)
    {
        _welcomeMessageIds.Add(messageId);
    }

    public bool IsWelcomeMessage(Snowflake messageId)
    {
        return _welcomeMessageIds.Contains(messageId);
    }

    public void AddUser(Snowflake instigator, Snowflake responder, Snowflake guildId, Snowflake channelId, DateTime userJoinedAt)
    {
        var instigatorData = _welcomerFeedbackUsers.FirstOrDefault(x => x.Instigator == instigator);
        if (instigatorData.Responders.Contains(responder))
        {
            _logger.LogWarning("Instigator {Instigator} cannot be welcomed or Responder {Responder} has already responded", instigator, responder);
            return;
        }
        
        if (instigator == responder)
        {
            _logger.LogWarning("Instigator {Instigator} cannot be welcomed or Responder {Responder} is the instigator", instigator, responder);
            return;
        }

        if (DateTime.UtcNow - userJoinedAt > TimeSpan.FromMinutes(5))
        {
            _logger.LogWarning("Responder {Responder}'s message is too old", responder);
            return;
        }

        var randomExperienceAmount = new Random().Next(5, 10);
        instigatorData.Responders.Add(responder);
        _mediator.Send(new GrantExperienceAmountToUserCommand(responder, guildId, channelId, randomExperienceAmount));

        _logger.LogInformation($"User {responder} has been added to the list of responders and has been granted {randomExperienceAmount} experience points");
    }

    public void RemoveUser(Snowflake instigator)
    {
        var user = _welcomerFeedbackUsers.FirstOrDefault(x => x.Instigator == instigator);
        if (user != null)
        {
            _welcomerFeedbackUsers.Remove(user);
        }
        else
        {
            _logger.LogWarning("Instigator {Instigator} cannot be found", instigator);
        }
    }
}