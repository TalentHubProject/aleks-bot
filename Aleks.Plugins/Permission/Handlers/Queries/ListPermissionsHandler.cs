// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Aleks.Business.Extensions;
using Aleks.Data;
using Aleks.Data.Domain.Permission;
using Aleks.Plugins.Permission.Queries;

namespace Aleks.Plugins.Permission.Handlers.Queries;

public sealed record ListPermissionsHandler(AleksDbContext DbContext)
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