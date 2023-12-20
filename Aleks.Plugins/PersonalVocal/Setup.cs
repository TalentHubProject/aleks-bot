// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Aleks.Plugins.PersonalVocal.CommandGroups;
using Aleks.Plugins.PersonalVocal.Responders;
using Aleks.Plugins.PersonalVocal.Services;
using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;
using Remora.Discord.Gateway.Extensions;

namespace Aleks.Plugins.PersonalVocal;

public static class Setup
{
    public static IServiceCollection AddPersonalVocalPlugin(this IServiceCollection serviceCollection)
    {
        return serviceCollection
                .AddCommandTree()
                .WithCommandGroup<SetUniqueVocalChannelPerGuildCommandGroup>()
                .Finish()
            
                .AddResponder<JoinPossibleVocalCreationResponder>()
            
                .AddSingleton<IPersonalVocalService, PersonalVocalService>()
            ;
    }
}