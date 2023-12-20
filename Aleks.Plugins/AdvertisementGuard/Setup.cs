// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Aleks.Plugins.AdvertisementGuard.Responders;
using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.Gateway.Extensions;

namespace Aleks.Plugins.AdvertisementGuard;

public static class Setup
{
    public static IServiceCollection AddAdvertisementGuardPlugin(this IServiceCollection services)
    {
        return services.AddResponder<UserMessageAdvertisementGuardResponder>();
    }
}