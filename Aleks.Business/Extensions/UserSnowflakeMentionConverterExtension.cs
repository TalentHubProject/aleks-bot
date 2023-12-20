using Remora.Rest.Core;

namespace Aleks.Business.Extensions;

/// <summary>
///     The extension methods for the <see cref="Snowflake" /> struct.
/// </summary>
public static class UserSnowflakeMentionConverterExtension
{
    /// <summary>
    ///     Converts a <see cref="Snowflake" /> to a mention.
    /// </summary>
    /// <param name="userSnowflake">The user snowflake.</param>
    /// <returns>The converted value.</returns>
    public static string ToMention(this Snowflake userSnowflake)
    {
        return $"<@{userSnowflake.Value}>";
    }
}