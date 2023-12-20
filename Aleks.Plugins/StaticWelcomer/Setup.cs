using Aleks.Plugins.StaticWelcomer.Responders;
using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.Gateway.Extensions;

namespace Aleks.Plugins.StaticWelcomer;

/// <summary>
///     Setup for the static welcomer plugin
/// </summary>
public static class Setup
{
    /// <summary>
    ///     Adds the static welcomer plugin to the service collection.
    /// </summary>
    /// <param name="serviceCollection">service collection</param>
    /// <returns>The builder.</returns>
    public static IServiceCollection AddStaticWelcomerPlugin(this IServiceCollection serviceCollection)
    {
        return serviceCollection
                .AddResponder<StaticWelcomerResponder>()
            ;
    }
}