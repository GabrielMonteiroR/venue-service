
using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models;

namespace venue_service.Src.Contexts;

public class VenueContext : DbContext
{
    public VenueContext(DbContextOptions<VenueContext> options) : base(options) { }

    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<VenueImage> VenueImages => Set<VenueImage>();
    public DbSet<VenueContactInfo> VenueContactInfos => Set<VenueContactInfo>();
    public DbSet<VenueAvailabilityTime> VenueAvailabilities => Set<VenueAvailabilityTime>();
    public DbSet<VenueStatus> VenueStatuses => Set<VenueStatus>();
    public DbSet<VenueType> VenueTypes => Set<VenueType>();
    public DbSet<Venue_Sport> VenueSports => Set<Venue_Sport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Venue>().ToTable("venues");
        modelBuilder.Entity<VenueImage>().ToTable("venue_images");
        modelBuilder.Entity<VenueContactInfo>().ToTable("venue_contact_infos");
        modelBuilder.Entity<VenueAvailabilityTime>().ToTable("venue_availability");
        modelBuilder.Entity<VenueStatus>().ToTable("venue_status");
        modelBuilder.Entity<VenueType>().ToTable("venue_types");
        modelBuilder.Entity<Venue_Sport>().ToTable("venue_sports");

        modelBuilder.Entity<VenueAvailabilityTime>().Property(v => v.Price).HasColumnType("numeric");
        modelBuilder.Entity<VenueAvailabilityTime>().Property(v => v.UserId).HasColumnName("reserved_by");

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

        modelBuilder.Entity<Venue>()
            .HasOne(v => v.Owner)
            .WithMany()
            .HasForeignKey(v => v.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Venue>()
            .HasOne(v => v.VenueType)
            .WithMany()
            .HasForeignKey(v => v.VenueTypeId);

        modelBuilder.Entity<Venue>()
            .HasMany(v => v.VenueEquipaments)
            .WithOne(ve => ve.Venue)
            .HasForeignKey(ve => ve.VenueId);

        modelBuilder.Entity<Venue>()
            .HasMany(v => v.VenueImages)
            .WithOne(vi => vi.Venue)
            .HasForeignKey(vi => vi.VenueId);

        modelBuilder.Entity<Venue>()
            .HasMany(v => v.VenueContactInfos)
            .WithOne(vc => vc.Venue)
            .HasForeignKey(vc => vc.VenueId);
    }
}
