
using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models;

namespace venue_service.Src.Contexts;

public class SportContext : DbContext
{
    public SportContext(DbContextOptions<SportContext> options) : base(options) { }

    public DbSet<Sport> Sports => Set<Sport>();
    public DbSet<Venue_Sport> VenueSports => Set<Venue_Sport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Sport>().ToTable("sports");
        modelBuilder.Entity<Venue_Sport>().ToTable("venue_sports");

        modelBuilder.Entity<Venue_Sport>()
            .HasKey(vs => new { vs.VenueId, vs.SportId });

        modelBuilder.Entity<Venue_Sport>()
            .HasOne(vs => vs.Venue)
            .WithMany(v => v.VenueSports)
            .HasForeignKey(vs => vs.VenueId);

        modelBuilder.Entity<Venue_Sport>()
            .HasOne(vs => vs.Sport)
            .WithMany()
            .HasForeignKey(vs => vs.SportId);
    }
}
