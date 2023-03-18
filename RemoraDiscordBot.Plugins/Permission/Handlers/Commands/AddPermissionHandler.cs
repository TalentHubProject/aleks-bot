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

namespace RemoraDiscordBot.Plugins.Permission.Handlers.Commands;

public class AddPermissionHandler
    : AsyncRequestHandler<AddPermissionCommand>
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly ILogger<AddPermissionHandler> _logger;

    public AddPermissionHandler(
        RemoraDiscordBotDbContext dbContext,
        ILogger<AddPermissionHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    protected override async Task Handle(AddPermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(
            x => x.Name == request.PermissionName
                 && x.GuildId == request.GuildId.ToLong(),
            cancellationToken);

        if (permission is null)
        {
            permission = new PermissionDto
            {
                Name = request.PermissionName,
                GuildId = request.GuildId.ToLong(),
                CategoryId = request.CategoryId.ToLong()
            };

            await _dbContext.Permissions.AddAsync(permission, cancellationToken);
        }

        _logger.LogInformation("Added permission {PermissionName} to guild {GuildId}.",
            request.PermissionName, request.GuildId);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}