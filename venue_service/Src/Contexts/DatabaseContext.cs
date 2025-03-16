using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models;

namespace venue_service.Src.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<VenueAvailability> VenueAvailabilities { get; set; }
        public DbSet<LocationAvailabilityTime> LocationAvailabilityTimes { get; set; }
        public DbSet<User_Venue> UserVenues { get; set; }
        public DbSet<Venue_Sport> VenueSports { get; set; }
        public DbSet<VenueEquipament> VenueEquipaments { get; set; }
        public DbSet<EquipamentBrand> EquipamentBrands { get; set; }
        public DbSet<EquipamentType> EquipamentTypes { get; set; }
        public DbSet<VenueImage> VenueImages { get; set; }
        public DbSet<VenueContactInfo> VenueContactInfos { get; set; }
        public DbSet<VenueStatus> VenueStatusEnums { get; set; }
        public DbSet<VenueType> VenueTypeEnums { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Role>().ToTable("roles");
            modelBuilder.Entity<Sport>().ToTable("sports");
            modelBuilder.Entity<Venue>().ToTable("venues");
            modelBuilder.Entity<VenueAvailability>().ToTable("venue_availability");
            modelBuilder.Entity<LocationAvailabilityTime>().ToTable("location_availability_times");
            modelBuilder.Entity<User_Venue>().ToTable("user_venues");
            modelBuilder.Entity<Venue_Sport>().ToTable("venue_sports");
            modelBuilder.Entity<VenueEquipament>().ToTable("venue_equipaments");
            modelBuilder.Entity<EquipamentBrand>().ToTable("equipament_brands");
            modelBuilder.Entity<EquipamentType>().ToTable("equipament_types");
            modelBuilder.Entity<VenueImage>().ToTable("venue_images");
            modelBuilder.Entity<VenueContactInfo>().ToTable("venue_contact_infos");
            modelBuilder.Entity<VenueStatus>().ToTable("venue_status");
            modelBuilder.Entity<VenueType>().ToTable("venue_types");
            modelBuilder.Entity<Reservation>().ToTable("reservations");

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User_Venue>()
                .HasKey(uv => new { uv.UserId, uv.VenueId });

            modelBuilder.Entity<User_Venue>()
                .HasOne(uv => uv.User)
                .WithMany(u => u.UserVenues)
                .HasForeignKey(uv => uv.UserId);

            modelBuilder.Entity<User_Venue>()
                .HasOne(uv => uv.Venue)
                .WithMany(v => v.UserVenues)
                .HasForeignKey(uv => uv.VenueId);


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
                .HasOne(v => v.VenueAvaliability)
                .WithOne(va => va.Venue)
                .HasForeignKey<VenueAvailability>(va => va.VenueId);

            modelBuilder.Entity<Venue>()
                .HasOne(v => v.VenueType)
                .WithMany()
                .HasForeignKey(v => v.VenueTypeId);

            modelBuilder.Entity<VenueAvailability>()
                .HasMany(va => va.LocationAvailabilityTimes)
                .WithOne(lat => lat.VenueAvailability)
                .HasForeignKey(lat => lat.VenueAvailabilityId);

            modelBuilder.Entity<LocationAvailabilityTime>()
                .HasOne(lat => lat.VenueStatus)
                .WithMany()
                .HasForeignKey(lat => lat.VenueStatusId);

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

            modelBuilder.Entity<EquipamentBrand>()
                .HasMany(eb => eb.VenueEquipaments)
                .WithOne(ve => ve.EquipamentBrand)
                .HasForeignKey(ve => ve.EquipamentBrandId);

            modelBuilder.Entity<EquipamentType>()
                .HasMany(et => et.VenueEquipaments)
                .WithOne(ve => ve.EquipamentType)
                .HasForeignKey(ve => ve.EquipamentTypeId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Venue)
                .WithMany()
                .HasForeignKey(r => r.VenueId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.LocationAvailabilityTime)
                .WithMany()
                .HasForeignKey(r => r.LocationAvailabilityTimeId);
        }
    }
}
