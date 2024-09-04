using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Base.Test.Domain;

public class TestDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    public DbSet<Domain.TestEntity> TestEntities { get; set; } = default!;
    public DbSet<Domain.TestUser> TestUsers { get; set; } = default!;

    public TestDbContext(DbContextOptions options) : base(options)
    {
        
    }
}