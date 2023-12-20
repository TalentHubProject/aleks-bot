// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Aleks.Business.Extensions;
using Aleks.Data;
using Aleks.Data.Domain.Permission;
using Aleks.Plugins.Permission.Commands;

namespace Aleks.Plugins.Permission.Handlers.Commands;

public class AddPermissionHandler
    : AsyncRequestHandler<AddPermissionCommand>
{
    private readonly AleksDbContext _dbContext;
    private readonly ILogger<AddPermissionHandler> _logger;
    private readonly IMediator _mediator;

    public AddPermissionHandler(
        AleksDbContext dbContext,
        ILogger<AddPermissionHandler> logger, 
        IMediator mediator)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mediator = mediator;
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