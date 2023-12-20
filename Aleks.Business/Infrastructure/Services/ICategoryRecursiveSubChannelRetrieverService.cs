using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest.Core;
using Remora.Results;

namespace Aleks.Business.Infrastructure.Services;

/// <summary>
///     The service to retrieve recursive sub channels of a category.
/// </summary>
public interface ICategoryRecursiveSubChannelRetrieverService
{
    /// <summary>
    ///     Retrieves the recursive sub channels of a category.
    /// </summary>
    /// <param name="guild">The guild.</param>
    /// <param name="categoryId">The category ID.</param>
    /// <returns>The readonly list of channels as a result.</returns>
    Task<Result<IReadOnlyList<IChannel>>> GetRecursiveAsync(IGuild guild, Snowflake categoryId);
}