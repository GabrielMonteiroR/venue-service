using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models;

namespace venue_service.Src.Contexts;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Sport> Sports { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<VenueAvailabilityTime> VenueAvailabilities { get; set; }
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
    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Role>().ToTable("roles");
        modelBuilder.Entity<Sport>().ToTable("sports");
        modelBuilder.Entity<Venue>().ToTable("venues");
        modelBuilder.Entity<VenueAvailabilityTime>().ToTable("venue_availability");
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
        modelBuilder.Entity<PaymentMethod>().ToTable("payment_methods");

        modelBuilder.Entity<VenueAvailabilityTime>().Property(v => v.Price).HasColumnType("numeric");

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
            .HasOne(r => r.PaymentMethod)
            .WithMany(pm => pm.Reservations)
            .HasForeignKey(r => r.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        // Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "Owner" },
            new Role { Id = 3, Name = "Athlete" }
        );

        modelBuilder.Entity<VenueStatus>().HasData(
            new VenueStatus
            {
                Id = 1,
                Name = "Available",
                Description = "Venue is available"
            },
            new VenueStatus
            {
                Id = 2,
                Name = "Maintenance",
                Description = "Under maintenance"
            },
            new VenueStatus
            {
                Id = 3,
                Name = "Unavailable",
                Description = "Not available"
            }
        );

        // Venue Type
        modelBuilder.Entity<VenueType>().HasData(
            new VenueType { Name = "Indoor court", Id = 1, Description = "Indoor court" },
            new VenueType { Name = "Outdoor field", Id = 2, Description = "Outdoor field" },
            new VenueType { Name = "Gymnasium", Id = 3, Description = "Gymnasium" }
        );

        // Payment Methods
        modelBuilder.Entity<PaymentMethod>().HasData(
            new PaymentMethod { Id = 1, Name = "CreditCard", Description = "Credit Card" },
            new PaymentMethod { Id = 2, Name = "Pix", Description = "Pix Payment" },
            new PaymentMethod { Id = 3, Name = "LocalPayment", Description = "Payment at Venue" }
        );

        // Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FirstName = "Gabriel",
                LastName = "Ricardo",
                Email = "gabriel@example.com",
                Password = "hashedpassword",
                Phone = "123456789",
                RoleId = 2,
                IsBanned = false,
                CreatedAt = new DateTime(2025, 03, 20, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 03, 20, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = 2,
                FirstName = "João",
                LastName = "Silva",
                Email = "joao@example.com",
                Password = "hashedpassword",
                Phone = "987654321",
                RoleId = 3,
                IsBanned = false,
                CreatedAt = new DateTime(2025, 03, 20, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 03, 20, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // Venues
        modelBuilder.Entity<Venue>().HasData(
            new Venue
            {
                Id = 1,
                Name = "Central Court",
                Address = "123 Main St",
                Capacity = 100,
                Latitude = -23.5505,
                Longitude = -46.6333,
                Description = "Main sports court",
                AllowLocalPayment = true,
                Rules = "No smoking",
                OwnerId = 1,
                VenueTypeId = 1,
                CreatedAt = new DateTime(2025, 03, 20, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 03, 20, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // Venue Availability
        modelBuilder.Entity<VenueAvailabilityTime>().HasData(
            new VenueAvailabilityTime
            {
                Id = 1,
                VenueId = 1,
                StartDate = new DateTime(2025, 03, 20, 08, 00, 00, DateTimeKind.Utc),
                EndDate = new DateTime(2025, 03, 20, 10, 00, 00, DateTimeKind.Utc),
                Price = 100,
                TimeStatus = "PENDING"
            }
        );

        // User_Venue (Owner)
        modelBuilder.Entity<User_Venue>().HasData(
            new User_Venue
            {
                UserId = 1,
                VenueId = 1
            }
        );

        // Sports
        modelBuilder.Entity<Sport>().HasData(
            new Sport { Id = 1, Name = "Football" },
            new Sport { Id = 2, Name = "Basketball" },
            new Sport { Id = 3, Name = "Tennis" }
        );

        // Equipament Brands
        modelBuilder.Entity<EquipamentBrand>().HasData(
            new EquipamentBrand { Id = 1, BrandName = "Nike" },
            new EquipamentBrand { Id = 2, BrandName = "Adidas" }
        );

        // Equipament Types
        modelBuilder.Entity<EquipamentType>().HasData(
            new EquipamentType { Id = 1, TypeName = "Ball" },
            new EquipamentType { Id = 2, TypeName = "Net" },
            new EquipamentType { Id = 3, TypeName = "Racket" }
        );

        // Venue Equipaments
        modelBuilder.Entity<VenueEquipament>().HasData(
            new VenueEquipament
            {
                Id = 1,
                EquipamentName = "Nike Football",
                EquipamentBrandId = 1,
                EquipamentTypeId = 1,
                Quantity = 10,
                VenueId = 1
            },
            new VenueEquipament
            {
                Id = 2,
                EquipamentName = "Adidas Net",
                EquipamentBrandId = 2,
                EquipamentTypeId = 2,
                Quantity = 2,
                VenueId = 1
            }
        );

        // Venue Contact Info
        modelBuilder.Entity<VenueContactInfo>().HasData(
            new VenueContactInfo
            {
                Id = 1,
                Phone = "999999999",
                Email = "contact@centralcourt.com",
                VenueId = 1
            }
        );

        // Venue Images
        modelBuilder.Entity<VenueImage>().HasData(
            new VenueImage
            {
                Id = 1,
                ImageURL = "https://example.com/image1.jpg",
                VenueId = 1
            }
        );

        modelBuilder.Entity<Reservation>().HasData(
            new Reservation
            {
                Id = 1,
                UserId = 1,
                VenueId = 1,
                VenueAvailabilityTimeId = 1,
                PaymentMethodId = 1,
                Status = "Pending",
                CreatedAt = new DateTime(2025, 03, 20, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // Venue_Sport (Many-to-Many)
        modelBuilder.Entity<Venue_Sport>().HasData(
            new Venue_Sport { VenueId = 1, SportId = 1 },
            new Venue_Sport { VenueId = 1, SportId = 2 }
        );
    }
}



