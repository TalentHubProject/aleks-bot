// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Conditions;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Messages;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;
using RemoraDiscordBot.Business.Colors;

namespace RemoraDiscordBot.Core.CommandGroups;

public class InformationalMessageCreatorCommandGroup
    : CommandGroup
{
    private readonly ICommandContext _commandContext;
    private readonly IDiscordRestGuildAPI _discordRestGuildApi;
    private readonly FeedbackService _feedbackService;

    public InformationalMessageCreatorCommandGroup(
        ICommandContext commandContext,
        FeedbackService feedbackService,
        IDiscordRestGuildAPI discordRestGuildApi)
    {
        _commandContext = commandContext;
        _feedbackService = feedbackService;
        _discordRestGuildApi = discordRestGuildApi;
    }

    [Command("informational-message")]
    [Description("Creates an informational message.")]
    [RequireDiscordPermission(DiscordPermission.Administrator)]
    public async Task<Result> InformationalMessageCreatorCommandAsync(string message)
    {
        if (!_commandContext.TryGetGuildID(out var guildId))
        {
            throw new InvalidOperationException("Could not get guild ID.");
        }

        var guild = await _discordRestGuildApi.GetGuildAsync(guildId.Value, ct: CancellationToken);
        var guildIconUri = CDN.GetGuildIconUrl(guild.Entity);

        return (Result) await _feedbackService.SendContextualEmbedAsync(
            new Embed
            {
                Title = "Informational Message",
                Description = message,
                Colour = DiscordTransparentColor.Value,
                Thumbnail = guildIconUri.IsSuccess
                    ? new EmbedThumbnail(guildIconUri.Entity.ToString())
                    : null
            },
            new FeedbackMessageOptions
            {
                MessageFlags = MessageFlags.Urgent
            },
            CancellationToken);
    }
}