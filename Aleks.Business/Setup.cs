using Aleks.Business.Infrastructure.Conditions;
using Aleks.Business.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;

namespace Aleks.Business;

/// <summary>
///     The setup class for the business layer.
/// </summary>
public static class Setup
{
    /// <summary>
    ///     Adds the business layer to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <returns>The service collection builder.</returns>
    public static IServiceCollection AddDiscordBotBusiness(this IServiceCollection serviceCollection)
    {
        return serviceCollection
                .AddCondition<NoBotCondition>()
                .AddCondition<NonSelfActionableCondition>()
                .AddScoped<ICategoryRecursiveSubChannelRetrieverService, CategoryRecursiveSubChannelRetrieverService>()
            ;
    }
}