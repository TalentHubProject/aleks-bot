// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.Permission.Queries;

namespace RemoraDiscordBot.Plugins.Permission.Handlers.Queries;

public record PermissionExistsHandler(RemoraDiscordBotDbContext DbContext)
    : IRequestHandler<PermissionExistsQuery, bool>
{
    public async Task<bool> Handle(PermissionExistsQuery request, CancellationToken cancellationToken)
    {
        return await DbContext.Permissions.FirstOrDefaultAsync(
            x =>
                x.Name == request.PermissionName
                && x.GuildId == request.GuildId.ToLong(),
            cancellationToken) is not null;
    }
}