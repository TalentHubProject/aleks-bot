// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.Experience;
using RemoraDiscordBot.Plugins.Experience.Queries;

namespace RemoraDiscordBot.Plugins.Experience.Handlers;

public sealed record GetLeaderBoardByLengthHandler(RemoraDiscordBotDbContext DbContext)
    : IRequestHandler<GetLeaderBoardQuery, IEnumerable<UserGuildXp>>
{
    public async Task<IEnumerable<UserGuildXp>> Handle(
        GetLeaderBoardQuery request,
        CancellationToken cancellationToken)
    {
        var leaderBoard = DbContext.UserGuildXps
            .Where(x => x.GuildId == request.GuildId.ToLong())
            .OrderByDescending(x => x.Level)
            .ToListAsync(cancellationToken);

        return new ReadOnlyCollection<UserGuildXp>(await leaderBoard);
    }
}