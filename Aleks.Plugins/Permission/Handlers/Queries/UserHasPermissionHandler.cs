// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Aleks.Business.Extensions;
using Aleks.Data;
using Aleks.Data.Domain.Permission;
using Aleks.Plugins.Permission.Queries;

namespace Aleks.Plugins.Permission.Handlers.Queries;

public record UserHasPermissionHandler(
        AleksDbContext DbContext,
        ILogger<UserHasPermissionHandler> Logger)
    : IRequestHandler<UserHasPermissionQuery, bool>
{
    public async Task<bool> Handle(UserHasPermissionQuery request, CancellationToken cancellationToken)
    {
        var user = await DbContext.PermissionUsers.FirstOrDefaultAsync(
            x =>
                x.UserId == request.UserId.ToLong()
                && x.GuildId == request.GuildId.ToLong(),
            cancellationToken);

        if (user is null)
        {
            user = new PermissionUser
            {
                UserId = request.UserId.ToLong(),
                GuildId = request.GuildId.ToLong()
            };

            Logger.LogInformation("Adding new user {UserId} to the database.", request.UserId);

            await DbContext.PermissionUsers.AddAsync(user, cancellationToken);
        }

        return user.Permissions.FirstOrDefault(
            x => x.Name.Equals(request.PermissionName, StringComparison.OrdinalIgnoreCase)) is not null;
    }
}