using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.Commands.Extensions;

namespace Aleks.Bot.Infrastructure;

/// <summary>
///     The extension methods for the <see cref="IServiceCollection" /> interface.
/// </summary>
public static class Setup
{
    /// <summary>
    ///     The extension method to include the bot infrastructure part.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <returns>The service collection with the bot infrastructure part.</returns>
    public static IServiceCollection AddDiscordBotInfrastructure(this IServiceCollection serviceCollection)
    {
        return serviceCollection
                .AddPostExecutionEvent<CommandPostExecutionEvent>()
                .AddPreparationErrorEvent<PreparationErrorEvent>()
            ;
    }
}