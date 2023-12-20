using Aleks.Bot.CommandGroups;
using Aleks.Bot.Infrastructure;
using Aleks.Bot.Responders.AnyResponderLogging;
using Aleks.Bot.Responders.SelfResponder;
using Aleks.Business;
using Aleks.Plugins.Experience;
using Aleks.Plugins.Permission;
using Aleks.Plugins.Welcomer;
using Aleks.Plugins.WelcomerFeedbackExperience;
using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Gateway;
using Remora.Discord.Interactivity.Extensions;

namespace Aleks.Bot;

/// <summary>
///     Setup for the Discord bot.
/// </summary>
public static class Setup
{
    /// <summary>
    ///     Adds the Discord bot to the service collection. It includes the default plugins.
    /// </summary>
    /// <param name="serviceCollection">service collection</param>
    /// <returns>The builder.</returns>
    public static IServiceCollection AddDiscordBot(
        this IServiceCollection serviceCollection)
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