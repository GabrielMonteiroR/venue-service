
using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models.Venue;

namespace venue_service.Src.Contexts;

public class EquipmentContext : DbContext
{
    public EquipmentContext(DbContextOptions<EquipmentContext> options) : base(options) { }

    public DbSet<VenueEquipamentEntity> VenueEquipaments => Set<VenueEquipamentEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Venue_SportEntity>();
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VenueEquipamentEntity>().ToTable("venue_equipaments");

        modelBuilder.Entity<VenueEquipamentEntity>()
            .HasOne(ve => ve.Venue)
            .WithMany(v => v.VenueEquipaments)
            .HasForeignKey(ve => ve.VenueId);
    }
}
