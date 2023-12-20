// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Rest;
using Aleks.Business.Extensions;
using Aleks.Business.Infrastructure.Services;
using Aleks.Data;
using Aleks.Plugins.Permission.Commands;

namespace Aleks.Plugins.Permission.Handlers.Commands;

public class RemoveUserDiscordPermissionHandler
    : AsyncRequestHandler<RemoveUserDiscordPermissionCommand>
{
    private readonly ICategoryRecursiveSubChannelRetrieverService _categoryRecursiveSubChannelRetrieverService;
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly AleksDbContext _dbContext;
    private readonly IDiscordRestGuildAPI _guildApi;
    private readonly ILogger<RemoveUserDiscordPermissionHandler> _logger;

    public RemoveUserDiscordPermissionHandler(
        AleksDbContext dbContext,
        IDiscordRestChannelAPI channelApi,
        ICategoryRecursiveSubChannelRetrieverService categoryRecursiveSubChannelRetrieverService,
        IDiscordRestGuildAPI guildApi,
        ILogger<RemoveUserDiscordPermissionHandler> logger)
    {
        _dbContext = dbContext;
        _channelApi = channelApi;
        _categoryRecursiveSubChannelRetrieverService = categoryRecursiveSubChannelRetrieverService;
        _guildApi = guildApi;
        _logger = logger;
    }

    protected override async Task Handle(
        RemoveUserDiscordPermissionCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.PermissionUsers.FirstOrDefaultAsync(
            x => x.UserId == request.UserId.ToLong()
                 && x.GuildId == request.GuildId.ToLong(),
            cancellationToken);

        if (user is null)
        {
            throw new InvalidOperationException(
                $"User {request.UserId} does not exist in guild {request.GuildId}.");
        }

        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(
            x => x.Name == request.PermissionName
                 && x.GuildId == request.GuildId.ToLong(),
            cancellationToken);

        if (permission is null)
        {
            throw new InvalidOperationException(
                $"Permission {request.PermissionName} does not exist in guild {request.GuildId}.");
        }

        var channel = await _channelApi.GetChannelAsync(permission.CategoryId.ToSnowflake(), cancellationToken);

        if (!channel.IsSuccess)
        {
            throw new InvalidOperationException(
                $"Permission {request.PermissionName} does not exist in guild {request.GuildId}.");
        }

        var guild = await _guildApi.GetGuildAsync(request.GuildId, ct: cancellationToken);
        if (!guild.IsSuccess)
        {
            throw new InvalidOperationException(
                $"Permission {request.PermissionName} does not exist in guild {request.GuildId}.");
        }

        var subChannels = await _categoryRecursiveSubChannelRetrieverService.GetRecursiveAsync(
            guild.Entity,
            channel.Entity.ID);

        foreach (var subChannel in subChannels.Entity)
        {
            await _channelApi.DeleteChannelPermissionAsync(
                subChannel.ID,
                request.UserId,
                "Permission removed",
                cancellationToken);
        }

        _logger.LogInformation(
            $"{user.Id} removed from {permission.Name} counting of total of {subChannels.Entity.Count} sub-channels.");
    }
}