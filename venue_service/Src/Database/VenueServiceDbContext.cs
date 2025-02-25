using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models;

public class VenueServiceDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Venues> Venues { get; set; }
    public DbSet<UserVenue> UserVenues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserVenue>()
            .HasKey(uv => new { uv.UserId, uv.VenueId });

        modelBuilder.Entity<UserVenue>()
            .HasOne(uv => uv.User)
            .WithMany(u => u.UserVenues)
            .HasForeignKey(uv => uv.UserId);

        modelBuilder.Entity<UserVenue>()
            .HasOne(uv => uv.Venue)
            .WithMany(v => v.UserVenues)
            .HasForeignKey(uv => uv.VenueId);
    }
}

