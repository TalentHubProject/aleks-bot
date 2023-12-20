using Microsoft.Extensions.Logging;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Services;
using Remora.Results;

namespace Aleks.Bot.Infrastructure;

/// <summary>
///     The event that is called after the execution of a command.
/// </summary>
public class CommandPostExecutionEvent(ILogger<CommandPostExecutionEvent> logger) : IPostExecutionEvent
{
    /// <summary>
    ///     The event that is called after the execution of a command.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="commandResult">The result of the command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Result" /> representing the result of the asynchronous operation.</returns>
    public Task<Result> AfterExecutionAsync(
        ICommandContext context,
        IResult commandResult,
        CancellationToken ct = default)
    {
        if (!commandResult.IsSuccess)
        {
            logger.LogError(
                "Command {CommandName} failed with error {Error}",
                context.Command.Command.Node.CommandMethod.Name,
                commandResult.Error);
        }

        return Task.FromResult(Result.FromSuccess());
    }
}