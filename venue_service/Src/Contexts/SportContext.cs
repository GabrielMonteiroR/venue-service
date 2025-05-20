
using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models.Sport;

namespace venue_service.Src.Contexts;

public class SportContext : DbContext
{
    public SportContext(DbContextOptions<SportContext> options) : base(options) { }

    public DbSet<SportEntity> Sports => Set<SportEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SportEntity>().ToTable("sports");
    }
}
