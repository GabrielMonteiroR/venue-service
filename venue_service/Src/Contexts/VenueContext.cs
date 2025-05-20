
using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models.Venue;

namespace venue_service.Src.Contexts;

public class VenueContext : DbContext
{
    public VenueContext(DbContextOptions<VenueContext> options) : base(options) { }

    public DbSet<VenueEntity> Venues => Set<VenueEntity>();
    public DbSet<VenueImageEntity> VenueImages => Set<VenueImageEntity>();
    public DbSet<VenueContactInfoEntity> VenueContactInfos => Set<VenueContactInfoEntity>();
    public DbSet<VenueAvailabilityTimeEntity> VenueAvailabilities => Set<VenueAvailabilityTimeEntity>();
    public DbSet<VenueStatusEntity> VenueStatuses => Set<VenueStatusEntity>();
    public DbSet<VenueTypeEntity> VenueTypes => Set<VenueTypeEntity>();
    public DbSet<Venue_SportEntity> VenueSports => Set<Venue_SportEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VenueEntity>().ToTable("venues");
        modelBuilder.Entity<VenueImageEntity>().ToTable("venue_images");
        modelBuilder.Entity<VenueContactInfoEntity>().ToTable("venue_contact_infos");
        modelBuilder.Entity<VenueAvailabilityTimeEntity>().ToTable("venue_availability");
        modelBuilder.Entity<VenueStatusEntity>().ToTable("venue_status");
        modelBuilder.Entity<VenueTypeEntity>().ToTable("venue_types");
        modelBuilder.Entity<Venue_SportEntity>().ToTable("venue_sports");

        modelBuilder.Entity<VenueAvailabilityTimeEntity>().Property(v => v.Price).HasColumnType("numeric");
        modelBuilder.Entity<VenueAvailabilityTimeEntity>().Property(v => v.UserId).HasColumnName("reserved_by");

        modelBuilder.Entity<Venue_SportEntity>()
            .HasKey(vs => new { vs.VenueId, vs.SportId });

        modelBuilder.Entity<Venue_SportEntity>()
            .HasOne(vs => vs.Venue)
            .WithMany(v => v.VenueSports)
            .HasForeignKey(vs => vs.VenueId);

        modelBuilder.Entity<Venue_SportEntity>()
            .HasOne(vs => vs.Sport)
            .WithMany()
            .HasForeignKey(vs => vs.SportId);

        modelBuilder.Entity<VenueEntity>()
            .HasOne(v => v.Owner)
            .WithMany()
            .HasForeignKey(v => v.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VenueEntity>()
            .HasOne(v => v.VenueType)
            .WithMany()
            .HasForeignKey(v => v.VenueTypeId);

        modelBuilder.Entity<VenueEntity>()
            .HasMany(v => v.VenueEquipaments)
            .WithOne(ve => ve.Venue)
            .HasForeignKey(ve => ve.VenueId);

        modelBuilder.Entity<VenueEntity>()
            .HasMany(v => v.VenueImages)
            .WithOne(vi => vi.Venue)
            .HasForeignKey(vi => vi.VenueId);

        modelBuilder.Entity<VenueEntity>()
            .HasMany(v => v.VenueContactInfos)
            .WithOne(vc => vc.Venue)
            .HasForeignKey(vc => vc.VenueId);
    }
}
