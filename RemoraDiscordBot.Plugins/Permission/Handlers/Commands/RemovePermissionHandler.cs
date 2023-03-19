// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.Permission.Commands;

namespace RemoraDiscordBot.Plugins.Permission.Handlers.Commands;

public class RemovePermissionHandler
    : AsyncRequestHandler<RemovePermissionCommand>
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly ILogger<RemovePermissionHandler> _logger;

    public RemovePermissionHandler(
        RemoraDiscordBotDbContext dbContext,
        ILogger<RemovePermissionHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    protected override async Task Handle(
        RemovePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var permissions = await _dbContext.Permissions
            .Where(permission => permission.GuildId == request.GuildId.ToLong())
            .ToListAsync(cancellationToken);

        var permission = permissions.FirstOrDefault(permission => permission.Name == request.PermissionName);
        if (permission is null)
        {
            _logger.LogWarning("Permission {PermissionName} not found.", request.PermissionName);
            return;
        }

        _dbContext.Permissions.Remove(permission);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Permission {PermissionName} removed.", request.PermissionName);
    }
}