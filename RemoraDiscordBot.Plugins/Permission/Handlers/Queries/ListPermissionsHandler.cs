// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.Permission;
using RemoraDiscordBot.Plugins.Permission.Queries;

namespace RemoraDiscordBot.Plugins.Permission.Handlers.Queries;

public sealed record ListPermissionsHandler(RemoraDiscordBotDbContext DbContext)
    : IRequestHandler<ListPermissionsQuery, IEnumerable<PermissionDto>>
{
    public async Task<IEnumerable<PermissionDto>> Handle(ListPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await DbContext.Permissions
            .Where(x => x.GuildId == request.GuildId.ToLong())
            .ToListAsync(cancellationToken);

        return permissions;
    }
}