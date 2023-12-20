// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest.Core;
using Remora.Results;

namespace Aleks.Business.Infrastructure.Services;

public interface ICategoryRecursiveSubChannelRetrieverService
{
    Task<Result<IReadOnlyList<IChannel>>> GetRecursiveAsync(IGuild guild, Snowflake categoryId);
}