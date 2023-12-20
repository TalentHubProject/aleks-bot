// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Aleks.Business.Extensions;
using Aleks.Data;
using Aleks.Data.Domain.Welcomer;
using Aleks.Plugins.Welcomer.Commands;

namespace Aleks.Plugins.Welcomer.Handlers.Commands;

public class CreateOrSetWelcomeChannelHandler
    : AsyncRequestHandler<CreateOrSetWelcomeChannelCommand>
{
    private readonly AleksDbContext _dbContext;

    public CreateOrSetWelcomeChannelHandler(AleksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task Handle(
        CreateOrSetWelcomeChannelCommand request,
        CancellationToken cancellationToken)
    {
        var guild = await _dbContext.WelcomerGuilds
            .FirstOrDefaultAsync(g => g.GuildId == request.GuildID.ToLong(), cancellationToken);

        if (guild is null)
        {
            _dbContext.WelcomerGuilds.Add(new WelcomerGuild
            {
                GuildId = request.GuildID.ToLong(),
                WelcomeChannelId = request.ChannelID.ToLong()
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return;
        }

        guild.WelcomeChannelId = request.ChannelID.ToLong();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}