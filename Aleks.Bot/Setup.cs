// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Aleks.Bot.CommandGroups;
using Aleks.Bot.Infrastructure;
using Aleks.Bot.Responders.AnyResponderLogging;
using Aleks.Bot.Responders.SelfResponder;
using Aleks.Business;
using Aleks.Plugins.Experience;
using Aleks.Plugins.Permission;
using Aleks.Plugins.Welcomer;
using Aleks.Plugins.WelcomerFeedbackExperience;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Gateway;
using Remora.Discord.Interactivity.Extensions;

namespace Aleks.Bot;

public static class Setup
{
    public static IServiceCollection AddDiscordBot(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        return serviceCollection
                .Configure<DiscordGatewayClientOptions>(options =>
                {
                    options.Intents = GatewayIntents.MessageContents
                                      | GatewayIntents.GuildMessages
                                      | GatewayIntents.Guilds
                                      | GatewayIntents.GuildMembers
                                      | GatewayIntents.GuildMessageReactions
                                      | GatewayIntents.GuildMessageTyping
                                      | GatewayIntents.Guilds
                                      | GatewayIntents.GuildVoiceStates;
                })
                .AddInteractivity()
                .AddDiscordCommands(true)
                .AddDiscordBotCoreCommands()
                .AddExperiencePlugin()
                .AddPermissionPlugin()
                .AddWelcomerPlugin()
                .AddAnyEventResponderLogging()
                .AddDiscordBotInfrastructure()
                .AddDiscordBotBusiness()
                .AddWelcomerFeedbackExperiencePlugin()
                .AddSelfResponder()
            ;
    }
}