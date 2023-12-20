using Aleks.Business.Infrastructure.Attributes;
using Aleks.Business.Infrastructure.Errors;
using Remora.Commands.Conditions;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Results;

namespace Aleks.Business.Infrastructure.Conditions;

/// <summary>
///     The condition to check if the user is not a bot.
/// </summary>
public class NoBotCondition
    : ICondition<NoBotAttribute, IUser>
{
    /// <inheritdoc />
    public ValueTask<Result> CheckAsync(NoBotAttribute attribute, IUser data, CancellationToken ct = default)
    {
        return ValueTask.FromResult(data?.IsBot is { HasValue: true, Value: true }
            ? Result.FromError(new NoBotError("This command cannot be used on bots."))
            : Result.FromSuccess());
    }
}