using Remora.Rest.Core;

namespace Aleks.Business.Extensions;

/// <summary>
///     The extension methods for the <see cref="Snowflake" /> struct.
/// </summary>
public static class SnowflakeConverterExtension
{
    /// <summary>
    ///     Converts a <see cref="long" /> to a <see cref="Snowflake" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The converted value.</returns>
    public static Snowflake ToSnowflake(this long value)
    {
        var ulongValue = unchecked((ulong)value);
        return new Snowflake(ulongValue);
    }

    /// <summary>
    ///     Converts a <see cref="Snowflake" /> to a <see cref="long" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The converted value.</returns>
    public static long ToLong(this Snowflake value)
    {
        return long.TryParse(value.ToString(), out var result) ? result : 0;
    }
}