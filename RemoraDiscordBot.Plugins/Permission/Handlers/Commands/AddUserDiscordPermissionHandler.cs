// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly IDiscordRestChannelAPI _discordRestChannelApi;
    private readonly IDiscordRestGuildAPI _discordRestGuildApi;
    private readonly IDiscordRestUserAPI _discordRestUserApi;
    private readonly ILogger<AddUserDiscordPermissionHandler> _logger;

    public AddUserDiscordPermissionHandler(
        IDiscordRestUserAPI discordRestUserApi,
        IDiscordRestChannelAPI discordRestChannelApi,
        ICategoryRecursiveSubChannelRetrieverService categoryRecursiveSubChannelRetrieverService,
        IDiscordRestGuildAPI discordRestGuildApi,
        ILogger<AddUserDiscordPermissionHandler> logger)
    {
        _discordRestUserApi = discordRestUserApi;
        _discordRestChannelApi = discordRestChannelApi;
        _categoryRecursiveSubChannelRetrieverService = categoryRecursiveSubChannelRetrieverService;
        _discordRestGuildApi = discordRestGuildApi;
        _logger = logger;
    }

    protected override async Task Handle(
        AddUserDiscordPermissionCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _discordRestUserApi.GetUserAsync(request.UserId, cancellationToken);

        if (!user.IsSuccess)
        {
            throw new InvalidOperationException("The user does not exist.");
        }

        var guild = await _discordRestGuildApi.GetGuildAsync(request.GuildId, ct: cancellationToken);
        if (!guild.IsSuccess)
        {
            throw new InvalidOperationException("The guild does not exist.");
        }

        var categoryId = request.PermissionDto.CategoryId.ToSnowflake();
        var categorySubChannels = await _categoryRecursiveSubChannelRetrieverService.GetRecursiveAsync(
            guild.Entity,
            categoryId);

        _logger.LogInformation(
            "Retrieved {CategorySubChannelsCount} sub channels for category {CategoryId}.",
            categorySubChannels.Entity.Count,
            categoryId);

        foreach (var categorySubChannel in categorySubChannels.Entity)
        {
            await _discordRestChannelApi.EditChannelPermissionsAsync(
                categorySubChannel.ID,
                request.UserId,
                new DiscordPermissionSet(
                    DiscordPermission.SendMessages,
                    DiscordPermission.ViewChannel),
                type: PermissionOverwriteType.Member,
                ct: cancellationToken);
        }
    }
}