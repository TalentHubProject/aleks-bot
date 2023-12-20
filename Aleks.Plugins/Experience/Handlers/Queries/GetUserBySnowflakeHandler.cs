// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Aleks.Business.Extensions;
using Aleks.Data;
using Aleks.Data.Domain.Experience;
using Aleks.Plugins.Experience.Queries;

namespace Aleks.Plugins.Experience.Handlers;

public sealed record GetUserBySnowflakeHandler(AleksDbContext DbContext)
    : IRequestHandler<GetUserBySnowflakeQuery, UserGuildXp?>
{
    public async Task<UserGuildXp?> Handle(GetUserBySnowflakeQuery request, CancellationToken cancellationToken)
    {
        var user = await DbContext.UserGuildXps.FirstOrDefaultAsync(x => x.UserId == request.UserSnowflake.ToLong(), cancellationToken);

        ArgumentNullException.ThrowIfNull(user);
        
        return user;
    }
}
