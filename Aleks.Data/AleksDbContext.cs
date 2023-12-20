// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Aleks.Data.Domain.AutoRoles;
using Aleks.Data.Domain.Experience;
using Aleks.Data.Domain.Permission;
using Aleks.Data.Domain.PersonalVocal;
using Aleks.Data.Domain.Welcomer;

namespace Aleks.Data;

public class AleksDbContext
    : DbContext
{
    public AleksDbContext(
        DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<UserGuildXp> UserGuildXps { get; set; } = null!;

    public DbSet<WelcomerGuild> WelcomerGuilds { get; set; } = null!;

    public DbSet<UserGuildCreature> UserGuildCreatures { get; set; } = null!;

    public DbSet<AutoRoleChannel> AutoRoleChannels { get; set; } = null!;
    public DbSet<AutoRoleReaction> AutoRoleReactions { get; set; } = null!;
    public DbSet<PersonalVocal> PersonalVocals { get; set; } = null!;
    public DbSet<PermissionDto> Permissions { get; set; } = null!;
    public DbSet<PermissionUser> PermissionUsers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userGuildXp = modelBuilder.Entity<UserGuildXp>();
        var autoRole = modelBuilder.Entity<AutoRoleChannel>();
        var personalVocal = modelBuilder.Entity<PersonalVocal>();
        var permissionUser = modelBuilder.Entity<PermissionUser>();

        userGuildXp
            .HasKey(x => new {x.UserId, x.GuildId});

        userGuildXp
            .HasOne(u => u.Creature)
            .WithOne(c => c.Possessor)
            .HasForeignKey<UserGuildCreature>(c => new {c.PossessorId, c.PossessorGuildId});

        modelBuilder.Entity<UserGuildCreature>()
            .HasKey(c => c.Id);

        autoRole
            .HasKey(x => new {x.MessageId, x.GuildId});

        modelBuilder.Entity<AutoRoleReaction>()
            .HasKey(x => x.Id);

        autoRole
            .HasMany(x => x.Reactions)
            .WithOne(x => x.AutoRoleChannel)
            .HasForeignKey(x => new {x.InstigatorMessageId, x.InstigatorGuildId});

        personalVocal
            .HasKey(x => x.GuildId);

        modelBuilder.Entity<PermissionDto>().HasKey(x => new {x.Id, x.Name, x.GuildId});

        permissionUser
            .HasMany(x => x.Permissions)
            ;
    }
    
    
}