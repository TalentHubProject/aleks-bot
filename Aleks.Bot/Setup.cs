// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Responders;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Extensions;
using Remora.Discord.Interactivity;
using Remora.Discord.Interactivity.Extensions;
using Aleks.Business;
using Aleks.Bot.CommandGroups;
using Aleks.Bot.Exceptions;
using Aleks.Bot.Infrastructure;
using Aleks.Bot.Responders.AnyResponderLogging;
using Aleks.Bot.Responders.SelfResponder;
using Aleks.Plugins.Experience;
using Aleks.Plugins.Permission;
using Aleks.Plugins.Welcomer;
using Aleks.Plugins.WelcomerFeedbackExperience;

namespace Aleks.Bot;

public static class Setup
{
    public static IServiceCollection AddDiscordBot(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var botToken = configuration["Discord:BotToken"]
                       ?? throw new BotTokenCannotBeNullException();

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