// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Discord.API;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Results;
using RemoraDiscordBot.Plugins.Permission.Queries;

namespace RemoraDiscordBot.Plugins.Permission.Handlers.Queries;

public record NotifyUserPermissionChangeHandler(
        IDiscordRestUserAPI UserApi,
        IDiscordRestChannelAPI ChannelApi,
        IDiscordRestGuildAPI GuildApi)
    : IRequestHandler<NotifyUserPermissionChangeQuery, Result>
{
    public async Task<Result> Handle(
        NotifyUserPermissionChangeQuery request,
        CancellationToken cancellationToken)
    {
        var user = await UserApi.GetUserAsync(request.UserId, cancellationToken);

        if (!user.IsSuccess)
        {
            return Result.FromError(user);
        }

        var dmChannel = await UserApi.CreateDMAsync(request.UserId, cancellationToken);

        if (!dmChannel.IsSuccess)
        {
            return Result.FromError(dmChannel);
        }

        var guild = await GuildApi.GetGuildAsync(request.GuildId, ct: cancellationToken);

        if (!guild.IsSuccess)
        {
            return Result.FromError(guild);
        }

        return (Result) await ChannelApi.CreateMessageAsync(
            dmChannel.Entity.ID,
            embeds: new[]
            {
                new Embed
                {
                    Title = "Permission change",
                    Description = $"You have been granted the permission `{request.PermissionName}`.",
                    Thumbnail = new EmbedThumbnail(CDN.GetGuildIconUrl(guild.Entity).ToString() ?? string.Empty)
                }
            },
            ct: cancellationToken);
    }
}