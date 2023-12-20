using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace Aleks.Business.Infrastructure.Services;

/// <summary>
///     The service to retrieve recursive sub channels of a category.
/// </summary>
/// <param name="discordRestGuildApi">The injected discord rest guild API.</param>
/// <param name="logger">The injected logger.</param>
public class CategoryRecursiveSubChannelRetrieverService(
    IDiscordRestGuildAPI discordRestGuildApi,
    ILogger<CategoryRecursiveSubChannelRetrieverService> logger)
    : ICategoryRecursiveSubChannelRetrieverService
{
    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<IChannel>>> GetRecursiveAsync(IGuild guild, Snowflake categoryId)
    {
        var guildChannels = await discordRestGuildApi.GetGuildChannelsAsync(guild.ID);
        if (!guildChannels.IsSuccess)
        {
            logger.LogError("Failed to retrieve guild channels: {Message}", guildChannels.Error.Message);
            return Result<IReadOnlyList<IChannel>>.FromError(guildChannels.Error);
        }

        var category = guildChannels.Entity.FirstOrDefault(x => x.ID == categoryId);
        if (category is null)
        {
            logger.LogWarning("Category with ID {CategoryId} not found.", categoryId);
            return Result<IReadOnlyList<IChannel>>.FromError(
                new NotFoundError($"Category with ID {categoryId} not found."));
        }

        var channels = guildChannels.Entity.Where(x => x.ParentID == category.ID).ToList().AsReadOnly();

        return Result<IReadOnlyList<IChannel>>.FromSuccess(channels);
    }
}