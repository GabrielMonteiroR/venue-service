
using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models;

namespace venue_service.Src.Contexts;

public class EquipamentContext : DbContext
{
    public EquipamentContext(DbContextOptions<EquipamentContext> options) : base(options) { }

    public DbSet<EquipamentType> EquipamentTypes => Set<EquipamentType>();
    public DbSet<VenueEquipament> VenueEquipaments => Set<VenueEquipament>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EquipamentType>().ToTable("equipament_types");
        modelBuilder.Entity<VenueEquipament>().ToTable("venue_equipaments");

        modelBuilder.Entity<EquipamentType>()
            .HasMany(et => et.VenueEquipaments)
            .WithOne(ve => ve.EquipamentType)
            .HasForeignKey(ve => ve.EquipamentTypeId);

        modelBuilder.Entity<VenueEquipament>()
            .HasOne(ve => ve.Venue)
            .WithMany(v => v.VenueEquipaments)
            .HasForeignKey(ve => ve.VenueId);
    }
}
