// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Business.Infrastructure.Services;
using RemoraDiscordBot.Plugins.Permission.Commands;

namespace RemoraDiscordBot.Plugins.Permission.Handlers.Commands;

public class AddUserDiscordPermissionHandler
    : AsyncRequestHandler<AddUserDiscordPermissionCommand>
{
    private readonly ICategoryRecursiveSubChannelRetrieverService _categoryRecursiveSubChannelRetrieverService;
    private readonly IDiscordRestChannelAPI _discordRestChannelAPI;
    private readonly IDiscordRestGuildAPI _discordRestGuildAPI;
    private readonly IDiscordRestUserAPI _discordRestUserAPI;

    public AddUserDiscordPermissionHandler(
        IDiscordRestUserAPI discordRestUserApi,
        IDiscordRestChannelAPI discordRestChannelApi,
        ICategoryRecursiveSubChannelRetrieverService categoryRecursiveSubChannelRetrieverService,
        IDiscordRestGuildAPI discordRestGuildApi)
    {
        _discordRestUserAPI = discordRestUserApi;
        _discordRestChannelAPI = discordRestChannelApi;
        _categoryRecursiveSubChannelRetrieverService = categoryRecursiveSubChannelRetrieverService;
        _discordRestGuildAPI = discordRestGuildApi;
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

        var guild = await _discordRestGuildAPI.GetGuildAsync(request.GuildId, ct: cancellationToken);
        if (!guild.IsSuccess)
        {
            throw new InvalidOperationException("The guild does not exist.");
        }

        var categoryId = request.PermissionDto.CategoryId.ToSnowflake();
        var categorySubChannels = await _categoryRecursiveSubChannelRetrieverService.GetRecursiveAsync(
            guild.Entity,
            categoryId);

        foreach (var categorySubChannel in categorySubChannels.Entity)
        {
            await _discordRestChannelAPI.EditChannelPermissionsAsync(
                categorySubChannel.ID,
                request.UserId,
                new DiscordPermissionSet(
                    DiscordPermission.SendMessages,
                    DiscordPermission.ViewChannel),
                ct: cancellationToken);
        }
    }
}