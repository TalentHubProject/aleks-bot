// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Aleks.Business.Extensions;
using Aleks.Data;
using Aleks.Plugins.Permission.Queries;

namespace Aleks.Plugins.Permission.Handlers.Queries;

public record GetUserPermissionHandler(AleksDbContext DbContext)
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