// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Aleks.Business.Extensions;
using Aleks.Data;
using Aleks.Plugins.Experience.Queries;

namespace Aleks.Plugins.Experience.Handlers;

public sealed record GetExperienceNeededByUserHandler(AleksDbContext DbContext)
    : IRequestHandler<GetExperienceNeededByUserQuery, long>
{
    public async Task<long> Handle(GetExperienceNeededByUserQuery request, CancellationToken cancellationToken)
    {
        var user = await DbContext.UserGuildXps.FirstOrDefaultAsync(x => x.UserId == request.UserId.ToLong()
                                                                         && x.GuildId == request.GuildId.ToLong(),
            cancellationToken);

        return user?.XpNeededToLevelUp ?? 0;
    }
}