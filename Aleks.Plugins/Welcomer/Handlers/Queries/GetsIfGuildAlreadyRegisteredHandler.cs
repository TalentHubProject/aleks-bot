// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Aleks.Business.Extensions;
using Aleks.Data;
using Aleks.Data.Domain.Welcomer;
using Aleks.Plugins.Welcomer.Queries;

namespace Aleks.Plugins.Welcomer.Handlers.Queries;

public sealed record GetsIfGuildAlreadyRegisteredHandler(AleksDbContext DbContext)
    : IRequestHandler<GetsIfGuildAlreadyRegisteredQuery, WelcomerGuild?>
{
    public async Task<WelcomerGuild?> Handle(
        GetsIfGuildAlreadyRegisteredQuery request,
        CancellationToken cancellationToken)
    {
        var guild = await DbContext.WelcomerGuilds
            .AsQueryable()
            .FirstOrDefaultAsync(g => g.GuildId == request.GuildId.ToLong(), cancellationToken);

        return guild;
    }
}