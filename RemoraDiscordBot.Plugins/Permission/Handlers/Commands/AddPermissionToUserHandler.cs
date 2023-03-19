// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.Permission;
using RemoraDiscordBot.Plugins.Permission.Commands;
using RemoraDiscordBot.Plugins.Permission.Queries;

namespace RemoraDiscordBot.Plugins.Permission.Handlers.Commands;

public class AddPermissionToUserHandler
    : AsyncRequestHandler<AddPermissionToUserCommand>
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly ILogger<AddPermissionToUserHandler> _logger;
    private readonly IMediator _mediator;

    public AddPermissionToUserHandler(
        RemoraDiscordBotDbContext dbContext,
        ILogger<AddPermissionToUserHandler> logger,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mediator = mediator;
    }

    protected override async Task Handle(
        AddPermissionToUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.PermissionUsers
            .FirstOrDefaultAsync(
                x => x.UserId == request.UserId.ToLong()
                     && x.GuildId == request.GuildId.ToLong(),
                cancellationToken);

        var permission = await _dbContext.Permissions
            .FirstOrDefaultAsync(
                x => x.Name == request.PermissionName
                     && x.GuildId == request.GuildId.ToLong(),
                cancellationToken);

        if (user is null)
        {
            user = new PermissionUser
            {
                UserId = request.UserId.ToLong(),
                GuildId = request.GuildId.ToLong()
            };

            _dbContext.PermissionUsers.Add(user);
            _logger.LogInformation("Created user {UserId} in guild {GuildId}.",
                request.UserId, request.GuildId);
        }

        user.Permissions.Add(permission);
        _logger.LogInformation("Added permission {PermissionName} to user {UserId} in guild {GuildId}.",
            request.PermissionName, request.UserId, request.GuildId);

        await _mediator.Send(new AddUserDiscordPermissionCommand(request.UserId, permission, request.GuildId), cancellationToken);
        await _mediator.Send(new NotifyUserPermissionChangeQuery(request.UserId, permission.Name, request.GuildId), cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}