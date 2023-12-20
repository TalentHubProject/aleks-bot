// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Aleks.Plugins.PersonalVocal.Model;
using Aleks.Plugins.PersonalVocal.Queries;
using Aleks.Plugins.PersonalVocal.Services;
using MediatR;
using Remora.Rest.Core;

namespace Aleks.Plugins.PersonalVocal.Handlers.Queries;

public sealed record GetUserPersonalVocalRequestHandler(IPersonalVocalService PersonalVocalService)
    : IRequestHandler<GetUserPersonalVocalRequest, Tuple<UserVocalChannel, Snowflake>?>
{
    public async Task<Tuple<UserVocalChannel, Snowflake>?> Handle(
        GetUserPersonalVocalRequest request,
        CancellationToken cancellationToken)
    {
        return PersonalVocalService.GetVoiceChannel(request.UserId, request.GuildId);
    }
}