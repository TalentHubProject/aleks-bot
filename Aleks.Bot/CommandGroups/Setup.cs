using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;

namespace Aleks.Bot.CommandGroups;

/// <summary>
///     The extension methods for the <see cref="IServiceCollection" /> interface.
/// </summary>
public static class Setup
{
    /// <summary>
    ///     The extension method to include the core commands.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <returns>The service collection with the core commands.</returns>
    public static IServiceCollection AddDiscordBotCoreCommands(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddCommandTree()
            .WithCommandGroup<CreditCommandGroup>()
            .WithCommandGroup<InformationalMessageCreatorCommandGroup>()
            .Finish()
            ;

        return serviceCollection;
    }
}