// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Remora.Discord.Commands.Feedback.Services;
using Aleks.Business.Extensions;
using Aleks.Data;
using Aleks.Plugins.Permission.Commands;

namespace Aleks.Plugins.Permission.Handlers.Commands;

public class RemovePermissionFromUserHandler
    : AsyncRequestHandler<RemovePermissionFromUserCommand>
{
    private readonly AleksDbContext _dbContext;
    private readonly FeedbackService _feedbackService;
    private readonly IMediator _mediator;

    public RemovePermissionFromUserHandler(
        AleksDbContext dbContext,
        FeedbackService feedbackService,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _feedbackService = feedbackService;
        _mediator = mediator;
    }

    protected override async Task Handle(
        RemovePermissionFromUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.PermissionUsers
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId.ToLong(),
                cancellationToken);

        if (user is null)
        {
            await _feedbackService.SendContextualErrorAsync(
                "The user does not have any permission.",
                ct: cancellationToken);

            throw new InvalidOperationException("The user does not have any permission.");
        }

        var permission = user.Permissions.FirstOrDefault(x => x.Name == request.PermissionName);

        if (permission is null)
        {
            await _feedbackService.SendContextualErrorAsync(
                "The user does not have this permission.",
                ct: cancellationToken);

            throw new InvalidOperationException("The user does not have this permission.");
        }

        user.Permissions.Remove(permission);

        await _mediator.Send(new RemoveUserDiscordPermissionCommand(
                request.UserId,
                request.GuildId,
                request.PermissionName),
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}