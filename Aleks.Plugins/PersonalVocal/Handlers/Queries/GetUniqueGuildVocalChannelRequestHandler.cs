// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Aleks.Business.Extensions;
using Aleks.Data;
using Aleks.Plugins.PersonalVocal.Queries;

namespace Aleks.Plugins.PersonalVocal.Handlers.Queries;

public sealed record GetUniqueGuildVocalChannelRequestHandler(AleksDbContext DbContext)
    : IRequestHandler<GetUniqueGuildVocalChannelRequest, Aleks.Data.Domain.PersonalVocal.PersonalVocal?>
{
    public async Task<Aleks.Data.Domain.PersonalVocal.PersonalVocal?> Handle(
        GetUniqueGuildVocalChannelRequest request,
        CancellationToken cancellationToken)
    {
        var channel = await DbContext.PersonalVocals
            .Where(x => x.GuildId == request.GuildId.ToLong())
            .FirstOrDefaultAsync(cancellationToken);

        return channel;
    }
}