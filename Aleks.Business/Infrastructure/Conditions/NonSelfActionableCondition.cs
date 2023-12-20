using Aleks.Business.Infrastructure.Attributes;
using Aleks.Business.Infrastructure.Errors;
using Remora.Commands.Conditions;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Results;

namespace Aleks.Business.Infrastructure.Conditions;

/// <summary>
///     The condition to check if the user is not itself.
/// </summary>
public class NonSelfActionableCondition
    : ICondition<NonSelfActionableAttribute, IUser>
{
    private readonly ICommandContext _commandContext;

    public NonSelfActionableCondition(
        ICommandContext commandContext)
    {
        _commandContext = commandContext;
    }

    // https://github.com/VelvetThePanda 
    // This code belongs to Velvet, I just adapted it to my needs.
    // It comes from the Velvet's Discord Bot Silk project.
    public async ValueTask<Result> CheckAsync(NonSelfActionableAttribute attribute, IUser data,
        CancellationToken ct = default)
    {
        var user = _commandContext switch
        {
            IInteractionContext interactionContext => interactionContext.Interaction.User.Value,
            ITextCommandContext textCommandContext => textCommandContext.Message.Author.Value,
            _ => throw new InvalidOperationException(),
        };

        return user.ID == data.ID
            ? Result.FromError(new NonSelfActionableError("You can't do this to yourself."))
            : Result.FromSuccess();
    }
}