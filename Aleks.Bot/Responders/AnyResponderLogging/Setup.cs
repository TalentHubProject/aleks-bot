using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.Gateway.Extensions;

namespace Aleks.Bot.Responders.AnyResponderLogging;

/// <summary>
///     Setup for the AnyEventResponder.
/// </summary>
public static class Setup
{
    /// <summary>
    ///     Adds the AnyEventResponder to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection builder.</returns>
    public static IServiceCollection AddAnyEventResponderLogging(this IServiceCollection services)
    {
        return services
            .AddResponder<AnyEventResponder>();
    }
}