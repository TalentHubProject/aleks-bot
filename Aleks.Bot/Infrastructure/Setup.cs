// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

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