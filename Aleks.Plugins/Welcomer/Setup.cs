// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Aleks.Plugins.Welcomer.CommandGroups;
using Aleks.Plugins.Welcomer.Responders;
using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;
using Remora.Discord.Gateway.Extensions;

namespace Aleks.Plugins.Welcomer;

public static class Setup
{
    public static IServiceCollection AddWelcomerPlugin(
        this IServiceCollection services)
    {
        return services
                .AddResponder<UserJoinGuildNotifierResponder>()
            
                .AddCommandTree()
                .WithCommandGroup<WelcomerConfigurationCommandGroup>()
                .Finish()
            ;
    }
}