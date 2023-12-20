using Aleks.Data.Domain.AutoRoles;
using Aleks.Data.Domain.Experience;
using Aleks.Data.Domain.Permission;
using Aleks.Data.Domain.PersonalVocal;
using Aleks.Data.Domain.Welcomer;
using Microsoft.EntityFrameworkCore;

namespace Aleks.Data;

/// <summary>
///     The database context.
/// </summary>
public class AleksDbContext
    : DbContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AleksDbContext" /> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public AleksDbContext(
        DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    ///     Gets or sets the user guild XP.
    /// </summary>
    public DbSet<UserGuildXp> UserGuildXps { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the welcomer guild.
    /// </summary>
    public DbSet<WelcomerGuild> WelcomerGuilds { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the user guild creature.
    /// </summary>
    public DbSet<UserGuildCreature> UserGuildCreatures { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the auto role channels.
    /// </summary>
    public DbSet<AutoRoleChannel> AutoRoleChannels { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the auto role reactions.
    /// </summary>
    public DbSet<AutoRoleReaction> AutoRoleReactions { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the personal vocals.
    /// </summary>
    public DbSet<PersonalVocal> PersonalVocals { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the permissions.
    /// </summary>
    public DbSet<PermissionDto> Permissions { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the permission users.
    /// </summary>
    public DbSet<PermissionUser> PermissionUsers { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the permission roles.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userGuildXp = modelBuilder.Entity<UserGuildXp>();
        var autoRole = modelBuilder.Entity<AutoRoleChannel>();
        var personalVocal = modelBuilder.Entity<PersonalVocal>();
        var permissionUser = modelBuilder.Entity<PermissionUser>();

        userGuildXp
            .HasKey(x => new { x.UserId, x.GuildId });

        userGuildXp
            .HasOne(u => u.Creature)
            .WithOne(c => c.Possessor)
            .HasForeignKey<UserGuildCreature>(c => new { c.PossessorId, c.PossessorGuildId });

        modelBuilder.Entity<UserGuildCreature>()
            .HasKey(c => c.Id);

        autoRole
            .HasKey(x => new { x.MessageId, x.GuildId });

        modelBuilder.Entity<AutoRoleReaction>()
            .HasKey(x => x.Id);

        autoRole
            .HasMany(x => x.Reactions)
            .WithOne(x => x.AutoRoleChannel)
            .HasForeignKey(x => new { x.InstigatorMessageId, x.InstigatorGuildId });

        personalVocal
            .HasKey(x => x.GuildId);

        modelBuilder.Entity<PermissionDto>().HasKey(x => new { x.Id, x.Name, x.GuildId });

        permissionUser
            .HasMany(x => x.Permissions)
            ;
    }
}