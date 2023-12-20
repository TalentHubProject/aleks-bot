using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.Gateway.Extensions;

namespace Aleks.Bot.Responders.SelfResponder;

/// <summary>
///     Setup for the SelfEventResponder.
/// </summary>
public static class Setup
{
    /// <summary>
    ///     Adds the SelfEventResponder to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <returns>The service collection builder.</returns>
    public static IServiceCollection AddSelfResponder(this IServiceCollection serviceCollection)
    {
        return serviceCollection
                .AddResponder<SelfEventResponder>()
            ;
    }
}