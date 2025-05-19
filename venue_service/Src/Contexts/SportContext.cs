
using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models;

namespace venue_service.Src.Contexts;

public class SportContext : DbContext
{
    public SportContext(DbContextOptions<SportContext> options) : base(options) { }

    public DbSet<Sport> Sports => Set<Sport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Sport>().ToTable("sports");
    }
}
