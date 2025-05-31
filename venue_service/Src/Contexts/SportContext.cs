
using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models.Sport;
using venue_service.Src.Models.Venue;

namespace venue_service.Src.Contexts;

public class SportContext : DbContext
{
    public SportContext(DbContextOptions<SportContext> options) : base(options) { }

    public DbSet<SportEntity> Sports => Set<SportEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<Venue_SportEntity>();


        modelBuilder.Entity<SportEntity>().ToTable("sports");
    }
}
