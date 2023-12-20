// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;

namespace Aleks.Data.Domain.Permission;

public sealed class PermissionUser
{
    [Key] public int Id { get; set; }

    public long UserId { get; set; }

    public long GuildId { get; set; }

    public ICollection<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
}