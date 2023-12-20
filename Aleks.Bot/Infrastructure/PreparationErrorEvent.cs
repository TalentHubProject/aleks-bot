using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Messages;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Discord.Commands.Services;
using Remora.Results;

namespace Aleks.Bot.Infrastructure;

/// <summary>
///     A preparation error event.
/// </summary>
public class PreparationErrorEvent(
    ILogger<PreparationErrorEvent> logger,
    FeedbackService feedbackService)
    : IPreparationErrorEvent
{
    /// <summary>
    ///     The event that is called when a command preparation fails.
    /// </summary>
    /// <param name="context">The operation context.</param>
    /// <param name="preparationResult">The result of the preparation.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Result" /> representing the result of the asynchronous operation.</returns>
    public async Task<Result> PreparationFailed(
        IOperationContext context,
        IResult preparationResult,
        CancellationToken ct = default)
    {
        if (preparationResult.IsSuccess)
        {
            return Result.FromSuccess();
        }

        if (!context.TryGetUserID(out var userId))
        {
            return Result.FromSuccess();
        }

        return (Result)await feedbackService.SendContextualErrorAsync(
            preparationResult.Inner?.Inner?.Error?.Message,
            userId.Value,
            new FeedbackMessageOptions
            {
                MessageFlags = MessageFlags.Ephemeral
            },
            ct);
    }
}