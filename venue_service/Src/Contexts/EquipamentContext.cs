
using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models;

namespace venue_service.Src.Contexts;

public class EquipamentContext : DbContext
{
    public EquipamentContext(DbContextOptions<EquipamentContext> options) : base(options) { }

    public DbSet<VenueEquipament> VenueEquipaments => Set<VenueEquipament>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Venue_Sport>();
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VenueEquipament>().ToTable("venue_equipaments");

        modelBuilder.Entity<VenueEquipament>()
            .HasOne(ve => ve.Venue)
            .WithMany(v => v.VenueEquipaments)
            .HasForeignKey(ve => ve.VenueId);
    }
}
