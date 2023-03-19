// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Conditions;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Rest.Core;
using Remora.Results;
using RemoraDiscordBot.Business.Colors;
using RemoraDiscordBot.Business.Infrastructure.Attributes;
using RemoraDiscordBot.Plugins.Permission.Commands;
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
    public class UserPermissionCommandGroupSub
        : CommandGroup
    {
        private readonly ICommandContext _commandContext;
        private readonly FeedbackService _feedbackService;
        private readonly IMediator _mediator;

        public UserPermissionCommandGroupSub(
            IMediator mediator,
            FeedbackService feedbackService,
            ICommandContext commandContext)
        {
            _mediator = mediator;
            _feedbackService = feedbackService;
            _commandContext = commandContext;
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
            if (!_commandContext.TryGetGuildID(out var guildId))
            {
                throw new InvalidOperationException("The command must be executed in a guild.");
            }

            var permissionExists = await _mediator.Send(new PermissionExistsQuery(permission, guildId.Value));
            if (!permissionExists)
            {
                return (Result) await _feedbackService.SendContextualErrorAsync(
                    $"The permission does not exist, ensure to create it with `/permission perm add {permission}`.");
            }

            var userHasPermission =
                await _mediator.Send(new UserHasPermissionQuery(user.ID, guildId.Value, permission));
            if (userHasPermission)
            {
                return (Result) await _feedbackService.SendContextualErrorAsync(
                    "The user already has the permission.");
            }

            await _mediator.Send(new AddPermissionToUserCommand(user.ID, guildId.Value, permission));

            return (Result) await _feedbackService.SendContextualSuccessAsync(
                $"The permission `{permission}` has been added to the user `{user.Username}`.");
        }
        
        [Command("remove")]
        [Description("Remove a permission from a user.")]
        [Ephemeral]
        public async Task<Result> RemoveUserPermissionCommandAsync(
            [Description("The user to remove the permission from.")][NoBot]
            IUser user,
            [Description("The permission name to remove from the user.")]
            string permission)
        {
            if (!_commandContext.TryGetGuildID(out var guildId))
            {
                throw new InvalidOperationException("The command must be executed in a guild.");
            }

            var permissionExists = await _mediator.Send(new PermissionExistsQuery(permission, guildId.Value));
            if (!permissionExists)
            {
                return (Result) await _feedbackService.SendContextualErrorAsync(
                    $"The permission does not exist, ensure to create it with `/permission perm add {permission}`.");
            }

            var userHasPermission =
                await _mediator.Send(new UserHasPermissionQuery(user.ID, guildId.Value, permission));
            if (!userHasPermission)
            {
                return (Result) await _feedbackService.SendContextualErrorAsync(
                    "The user does not have the permission.");
            }

            await _mediator.Send(new RemovePermissionFromUserCommand(user.ID, guildId.Value, permission));

            return (Result) await _feedbackService.SendContextualSuccessAsync(
                $"The permission `{permission}` has been removed from the user `{user.Username}`.");
        }
    }

    [Group("perm")]
    [Description("Commands to manage the permission of a permission.")]
    public class PermissionPermissionCommandGroupSub
        : CommandGroup
    {
        private readonly ICommandContext _commandContext;
        private readonly FeedbackService _feedbackService;
        private readonly IMediator _mediator;

        public PermissionPermissionCommandGroupSub(
            ICommandContext commandContext,
            IMediator mediator,
            FeedbackService feedbackService)
        {
            _commandContext = commandContext;
            _mediator = mediator;
            _feedbackService = feedbackService;
        }

        [Command("add")]
        [Description("Add a permission to a category.")]
        [Ephemeral]
        public async Task<Result> AddPermissionCommandAsync(
            [Description("The permission name to add.")]
            string permission,
            [Description("The category to add the permission to.")]
            Snowflake category)
        {
            if (!_commandContext.TryGetGuildID(out var guildId))
            {
                throw new InvalidOperationException("The command must be executed in a guild.");
            }

            await _mediator.Send(new AddPermissionCommand(guildId.Value, permission, category));

            return (Result) await _feedbackService.SendContextualSuccessAsync(
                $"The permission `{permission}` has been added to the category **<#{category}>**.");
        }

        [Command("list")]
        [Description("List all the permissions.")]
        [Ephemeral]
        public async Task<Result> ListPermissionCommandAsync()
        {
            if (!_commandContext.TryGetGuildID(out var guildId))
            {
                throw new InvalidOperationException("The command must be executed in a guild.");
            }

            var permissions = await _mediator.Send(new ListPermissionsQuery(guildId.Value));

            var embed = new Embed
            {
                Title = "Permissions",
                Colour = DiscordTransparentColor.Value,
                Description = string.Join("\n", permissions.Select(x => $"- `{x.Name}` - **<#{x.CategoryId}>**"))
            };

            return (Result) await _feedbackService.SendContextualEmbedAsync(embed);
        }
    }
}