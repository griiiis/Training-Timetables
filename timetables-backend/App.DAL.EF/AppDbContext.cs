using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>,
     AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public DbSet<Contest> Contests { get; set; } = default!;
    public DbSet<ContestGameType> ContestGameTypes { get; set; } = default!;
    public DbSet<ContestLevel> ContestLevels { get; set; } = default!;
    public DbSet<ContestPackage> ContestPackages { get; set; } = default!;
    public DbSet<ContestTime> ContestTimes { get; set; } = default!;
    public DbSet<Location> Locations { get; set; } = default!;
    public DbSet<ContestType> ContestTypes { get; set; } = default!;
    public DbSet<Court> Courts { get; set; } = default!;
    public DbSet<Game> Games { get; set; } = default!;
    public DbSet<GameType> GameTypes { get; set; } = default!;
    public DbSet<Level> Levels { get; set; } = default!;
    public DbSet<PackageGameTypeTime> PackageGameTypeTimes { get; set; } = default!;
    public DbSet<AppRole> AppRoles { get; set; } = default!;
    public DbSet<AppUser> AppUsers { get; set; } = default!;
    public DbSet<AppUserRole> AppUserRoles { get; set; } = default!;
    public DbSet<RolePreference> RolePreferences { get; set; } = default!;
    public DbSet<Team> Teams { get; set; } = default!;
    public DbSet<TeamGame> TeamGames { get; set; } = default!;
    public DbSet<Time> Times { get; set; } = default!;
    public DbSet<TimeOfDay> TimeOfDays { get; set; } = default!;
    public DbSet<TimeTeam> TimeTeams { get; set; } = default!;
    public DbSet<UserContestPackage> UserContestPackages { get; set; } = default!;
    public DbSet<AppRefreshToken> RefreshTokens { get; set; } = default!;

    public DbSet<ContestUserRole> ContestUserRoles { get; set; } = default!;
    public DbSet<ContestRole> ContestRoles { get; set; } = default!;
    public DbSet<Invitation> Invitations { get; set; } = default!;
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
        
        
    }
    
    
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // If using in memory database (not recommended!) modify and add the following to AppDbContext OnModelCreating method:
        if (this.Database.ProviderName!.Contains("Sqlite"))
        {
            builder.Entity<Contest>()
                .OwnsOne(e => e.ContestName, builder => { builder.ToJson(); });
            builder.Entity<Contest>()
                .OwnsOne(e => e.Description, builder => { builder.ToJson(); });
            builder.Entity<ContestType>()
                .OwnsOne(e => e.ContestTypeName, builder => { builder.ToJson(); });
            builder.Entity<ContestType>()
                .OwnsOne(e => e.Description, builder => { builder.ToJson(); });
            builder.Entity<Court>()
                .OwnsOne(e => e.CourtName, builder => { builder.ToJson(); });
            builder.Entity<Game>()
                .OwnsOne(e => e.Title, builder => { builder.ToJson(); });
            builder.Entity<GameType>()
                .OwnsOne(e => e.GameTypeName, builder => { builder.ToJson(); });
            builder.Entity<Level>()
                .OwnsOne(e => e.Title, builder => { builder.ToJson(); });
            builder.Entity<Level>()
                .OwnsOne(e => e.Description, builder => { builder.ToJson(); });
            builder.Entity<Location>()
                .OwnsOne(e => e.LocationName, builder => { builder.ToJson(); });
            builder.Entity<Location>()
                .OwnsOne(e => e.State, builder => { builder.ToJson(); });
            builder.Entity<Location>()
                .OwnsOne(e => e.Country, builder => { builder.ToJson(); });
            builder.Entity<PackageGameTypeTime>()
                .OwnsOne(e => e.PackageGtName, builder => { builder.ToJson(); });
            builder.Entity<Team>()
                .OwnsOne(e => e.TeamName, builder => { builder.ToJson(); });
            builder.Entity<TimeOfDay>()
                .OwnsOne(e => e.TimeOfDayName, builder => { builder.ToJson(); });
            builder.Entity<PackageGameTypeTime>()
                .OwnsOne(e => e.PackageGtName, builder => { builder.ToJson(); });
            builder.Entity<PackageGameTypeTime>()
                .OwnsOne(e => e.PackageGtName, builder => { builder.ToJson(); });
        }

        // disable cascade delete
        foreach (var relationship in builder.Model
                     .GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entity in ChangeTracker.Entries().Where(e => e.State != EntityState.Deleted))
        {
            foreach (var prop in entity.Properties
                         .Where(x => x.Metadata.ClrType == typeof(DateTime)))
            {
                Console.WriteLine(prop);
                prop.CurrentValue = ((DateTime)prop.CurrentValue).ToUniversalTime();
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}