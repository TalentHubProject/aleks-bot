using Aleks.Plugins.StaticStarboard.Responders;
using Aleks.Plugins.StaticStarboard.Services;
using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.Gateway.Extensions;

namespace Aleks.Plugins.StaticStarboard;

/// <summary>
///     This class is used to setup the plugin.
/// </summary>
public static class Setup
{
    /// <summary>
    ///     This method is used to add the static starboard plugin to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add the plugin to.</param>
    /// <returns>The service collection with the plugin added.</returns>
    public static IServiceCollection AddStaticStarboardPlugin(this IServiceCollection serviceCollection)
    {
        return serviceCollection
                .AddTransient<IStaticStarboardService, StaticStarboardDiscordService>()
                .AddResponder<StaticStarboardOnEnoughStarsGotResponder>()
            ;
    }
}