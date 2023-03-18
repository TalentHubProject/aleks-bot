// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Conditions;
using Remora.Discord.Commands.Feedback.Services;

namespace RemoraDiscordBot.Plugins.Permission.CommandGroups;

[Group("permission")]
[Description("Plugin to manage the permission feature.")]
[RequireDiscordPermission(DiscordPermission.Administrator)]
public class PermissionCommandGroup
    : CommandGroup
{
    private readonly FeedbackService _feedbackService;
    private readonly IMediator _mediator;

    public PermissionCommandGroup(
        IMediator mediator,
        FeedbackService feedbackService)
    {
        _mediator = mediator;
        _feedbackService = feedbackService;
    }
    
    [Group("user")]
    [Description("Commands to manage the permission of a user.")]
    internal class UserPermissionCommandGroupSub
    {
    }

    [Group("perm")]
    [Description("Commands to manage the permission of a permission.")]
    internal class PermissionPermissionCommandGroupSub
    {
    }
}