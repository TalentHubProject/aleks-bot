using Remora.Commands.Conditions;

namespace Aleks.Business.Infrastructure.Attributes;

/// <summary>
///     The attribute to use to mark a parameter as not being a bot.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class NoBotAttribute
    : ConditionAttribute
{
}