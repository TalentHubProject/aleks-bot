using Microsoft.EntityFrameworkCore;

namespace Aleks.Business.Extensions;

/// <summary>
///     The extension methods for the <see cref="DbContext" /> interface.
/// </summary>
public static class AddOrUpdateExtension
{
    /// <summary>
    ///     Adds or updates the entity.
    /// </summary>
    /// <param name="ctx">DbContext instance.</param>
    /// <param name="entity">The entity.</param>
    /// <exception cref="ArgumentOutOfRangeException">The entity state is not supported.</exception>
    public static void AddOrUpdate(this DbContext ctx, object entity)
    {
        var entry = ctx.Entry(entity);
        switch (entry.State)
        {
            case EntityState.Detached:
                ctx.Add(entity);
                break;
            case EntityState.Modified:
                ctx.Update(entity);
                break;
            case EntityState.Added:
                ctx.Add(entity);
                break;
            case EntityState.Unchanged:
                break;

            case EntityState.Deleted:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}