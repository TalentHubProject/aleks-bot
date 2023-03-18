// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Results;
using RemoraDiscordBot.Business.Infrastructure.Attributes;

namespace RemoraDiscordBot.Plugins.Permission.CommandGroups;

[Group("permission")]
[Description("Plugin to manage the permission feature.")]
public class PermissionCommandGroup
    : CommandGroup
{
    [Command("add")]
    [Description("Adds a permission to an user.")]
    public async Task<Result> AddPermissionCommandAsync(
        [Description("The user to add the permission to.")] [NoBot]
        IUser user,
        [Description("The permission to add to the user.")]
        string permission)
    {
        return Result.FromSuccess();
    }
}