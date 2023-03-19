// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Plugins.Permission.Commands;

namespace RemoraDiscordBot.Plugins.Permission.Handlers.Commands;

public class AddUserDiscordPermissionHandler
    : AsyncRequestHandler<AddUserDiscordPermissionCommand>
{
    private readonly IDiscordRestChannelAPI _discordRestChannelAPI;
    private readonly IDiscordRestUserAPI _discordRestUserAPI;

    public AddUserDiscordPermissionHandler(
        IDiscordRestUserAPI discordRestUserApi,
        IDiscordRestChannelAPI discordRestChannelApi)
    {
        _discordRestUserAPI = discordRestUserApi;
        _discordRestChannelAPI = discordRestChannelApi;
    }

    protected override async Task Handle(
        AddUserDiscordPermissionCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _discordRestUserAPI.GetUserAsync(request.UserId, cancellationToken);

        if (!user.IsSuccess)
        {
            throw new InvalidOperationException("The user does not exist.");
        }

        var channelId = request.PermissionDto.CategoryId.ToSnowflake();

        var editChannelPermissionsAsync = await _discordRestChannelAPI.EditChannelPermissionsAsync(
            channelId,
            request.UserId,
            new DiscordPermissionSet(
                DiscordPermission.Administrator),
            type: PermissionOverwriteType.Member,
            ct: cancellationToken);

        if (!editChannelPermissionsAsync.IsSuccess)
        {
            throw new InvalidOperationException("Cannot add permission for reason: " +
                                                editChannelPermissionsAsync.Inner?.Error?.Message);
        }
    }
}