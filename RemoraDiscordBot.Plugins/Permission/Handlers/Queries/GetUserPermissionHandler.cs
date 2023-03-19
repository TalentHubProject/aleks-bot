// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.Permission.Queries;

namespace RemoraDiscordBot.Plugins.Permission.Handlers.Queries;

public record GetUserPermissionHandler(RemoraDiscordBotDbContext DbContext)
    : IRequestHandler<GetUserPermissionQuery, IReadOnlyList<string>>
{
    public async Task<IReadOnlyList<string>> Handle(GetUserPermissionQuery request, CancellationToken cancellationToken)
    {
        var user = await DbContext.PermissionUsers
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(
                x => x.UserId == request.UserId.ToLong()
                     && x.GuildId == request.GuildId.ToLong(),
                cancellationToken);

        return user?.Permissions.Select(x => x.Name).ToList() 
               ?? new List<string>();
    }
}