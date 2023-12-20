// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Services;
using Remora.Results;

namespace Aleks.Bot.Infrastructure;

/// <summary>
///     The event that is called after the execution of a command.
/// </summary>
public class CommandPostExecutionEvent
    : IPostExecutionEvent
{
    private readonly ILogger<CommandPostExecutionEvent> _logger;

    public CommandPostExecutionEvent(ILogger<CommandPostExecutionEvent> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     The event that is called after the execution of a command.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="commandResult">The result of the command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Result" /> representing the result of the asynchronous operation.</returns>
    public async Task<Result> AfterExecutionAsync(
        ICommandContext context,
        IResult commandResult,
        CancellationToken ct = default)
    {
        if (!commandResult.IsSuccess)
        {
            _logger.LogError(
                "Command {CommandName} failed with error {Error}",
                context.Command.Command.Node.CommandMethod.Name,
                commandResult.Error);
        }

        return Result.FromSuccess();
    }
}