// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Rest.Core;

namespace Aleks.Plugins.Permission.Commands;

public record RemovePermissionCommand(string PermissionName, Snowflake GuildId)
    : IRequest;