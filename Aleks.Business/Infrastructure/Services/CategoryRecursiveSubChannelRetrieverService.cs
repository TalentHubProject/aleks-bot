// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace Aleks.Business.Infrastructure.Services;

public class CategoryRecursiveSubChannelRetrieverService
    : ICategoryRecursiveSubChannelRetrieverService
{
    private readonly IDiscordRestGuildAPI _discordRestGuildApi;
    private readonly ILogger<CategoryRecursiveSubChannelRetrieverService> _logger;

    public CategoryRecursiveSubChannelRetrieverService(
        IDiscordRestGuildAPI discordRestGuildApi,
        ILogger<CategoryRecursiveSubChannelRetrieverService> logger)
    {
        _discordRestGuildApi = discordRestGuildApi;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<IChannel>>> GetRecursiveAsync(IGuild guild, Snowflake categoryId)
    {
        var guildChannels = await _discordRestGuildApi.GetGuildChannelsAsync(guild.ID);
        if (!guildChannels.IsSuccess)
        {
            _logger.LogError("Failed to retrieve guild channels: {Message}", guildChannels.Error.Message);
            return Result<IReadOnlyList<IChannel>>.FromError(guildChannels.Error);
        }

        var category = guildChannels.Entity.FirstOrDefault(x => x.ID == categoryId);
        if (category is null)
        {
            _logger.LogWarning("Category with ID {CategoryId} not found.", categoryId);
            return Result<IReadOnlyList<IChannel>>.FromError(
                new NotFoundError($"Category with ID {categoryId} not found."));
        }

        var channels = guildChannels.Entity.Where(x => x.ParentID == category.ID).ToList().AsReadOnly();

        return Result<IReadOnlyList<IChannel>>.FromSuccess(channels);
    }
}