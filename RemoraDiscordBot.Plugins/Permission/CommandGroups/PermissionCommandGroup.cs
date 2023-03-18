// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Conditions;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;
using RemoraDiscordBot.Plugins.Permission.Queries;

namespace RemoraDiscordBot.Plugins.Permission.CommandGroups;

[Group("permission")]
[Description("Plugin to manage the permission feature.")]
[RequireDiscordPermission(DiscordPermission.Administrator)]
public class PermissionCommandGroup
    : CommandGroup
{
    [Group("user")]
    [Description("Commands to manage the permission of a user.")]
    internal class UserPermissionCommandGroupSub
        : CommandGroup
    {
        private readonly IMediator _mediator;
        private readonly FeedbackService _feedbackService;
        private readonly ITextCommandContext _textCommandContext;

        public UserPermissionCommandGroupSub(
            IMediator mediator,
            FeedbackService feedbackService,
            ITextCommandContext textCommandContext)
        {
            _mediator = mediator;
            _feedbackService = feedbackService;
            _textCommandContext = textCommandContext;
        }

        [Command("add")]
        [Description("Add a permission to a user.")]
        [Ephemeral]
        public async Task<Result> AddUserPermissionCommandAsync(
            [Description("The user to add the permission to.")]
            IUser user,
            [Description("The permission name to add to the user.")]
            string permission)
        {
            if (!_textCommandContext.TryGetGuildID(out var guildId))
            {
                throw new InvalidOperationException("The command must be executed in a guild.");
            }
            
            var permissionExists = await _mediator.Send(new PermissionExistsQuery(permission, guildId.Value));
            if (!permissionExists)
            {
                return (Result) await _feedbackService.SendContextualErrorAsync(
                    "The permission does not exist.");
            }
           /** 
            var userHasPermission = await _mediator.Send(new UserHasPermissionRequest(user.ID, permission));
            if (userHasPermission)
            {
                return (Result) await _feedbackService.SendContextualErrorAsync(
                    "The user already has the permission.");
            }
            
            var addPermissionToUser = await _mediator.Send(new AddPermissionToUserRequest(user.ID, permission));
            if (!addPermissionToUser)
            {
                return (Result) await _feedbackService.SendContextualErrorAsync(
                    "An error occurred while adding the permission to the user.");
            }**/
           return Result.FromSuccess();
        }
    }

    [Group("perm")]
    [Description("Commands to manage the permission of a permission.")]
    internal class PermissionPermissionCommandGroupSub
    {
    }
}