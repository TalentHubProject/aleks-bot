// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Rest.Core;
using Aleks.Data.Domain.PersonalVocal;
using Aleks.Plugins.PersonalVocal.Model;

namespace Aleks.Plugins.PersonalVocal.Queries;

public sealed record GetUserPersonalVocalRequest(
        Snowflake UserId,
        Snowflake GuildId)
    : IRequest<Tuple<UserVocalChannel, Snowflake>?>;