// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;

namespace RemoraDiscordBot.Data.Domain.Permission;

public sealed class PermissionDto
{
    [Key] public int Id { get; set; }
    
    public long GuildId { get; set; }

    public string Name { get; set; }

    public long CategoryId { get; set; }
}