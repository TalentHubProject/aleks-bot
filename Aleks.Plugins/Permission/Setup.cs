// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Aleks.Plugins.Permission.CommandGroups;
using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;

namespace Aleks.Plugins.Permission;

public static class Setup
{
    public static IServiceCollection AddPermissionPlugin(this IServiceCollection serviceCollection)
    {
        return serviceCollection
                .AddCommandTree()
                .WithCommandGroup<PermissionCommandGroup>()
                .Finish()
            ;
    }
}