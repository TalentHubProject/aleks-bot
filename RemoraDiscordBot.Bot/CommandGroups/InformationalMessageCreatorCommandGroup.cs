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
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;
using RemoraDiscordBot.Business.Colors;

namespace RemoraDiscordBot.Core.CommandGroups;

public class InformationalMessageCreatorCommandGroup
    : CommandGroup
{
    private readonly ICommandContext _commandContext;
    private readonly IDiscordRestChannelAPI _discordRestChannelApi;
    private readonly IDiscordRestGuildAPI _discordRestGuildApi;

    public InformationalMessageCreatorCommandGroup(
        ICommandContext commandContext,
        IDiscordRestGuildAPI discordRestGuildApi,
        IDiscordRestChannelAPI discordRestChannelApi)
    {
        _commandContext = commandContext;
        _discordRestGuildApi = discordRestGuildApi;
        _discordRestChannelApi = discordRestChannelApi;
    }

    [Command("informational-message")]
    [Description("Creates an informational message.")]
    [RequireDiscordPermission(DiscordPermission.Administrator)]
    public async Task<Result> InformationalMessageCreatorCommandAsync(
        string title,
        string message)
    {
        if (!_commandContext.TryGetGuildID(out var guildId))
        {
            throw new InvalidOperationException("Could not get guild ID.");
        }

        if (!_commandContext.TryGetChannelID(out var channelId))
        {
            throw new InvalidOperationException("Could not get channel ID.");
        }

        var guild = await _discordRestGuildApi.GetGuildAsync(guildId.Value, ct: CancellationToken);
        var guildIconUri = CDN.GetGuildIconUrl(guild.Entity);

        message = message.Replace("|", "\n");

        await _discordRestChannelApi.CreateMessageAsync(
            channelId.Value,
            embeds: new[]
            {
                new Embed
                {
                    Title = title,
                    Description = message,
                    Colour = DiscordTransparentColor.LogoColor
                }
            },
            ct: CancellationToken);
        
        return Result.FromSuccess();
    }
}