using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models;

public class VenueServiceDbContext : DbContext
{
    //TODO: Ajustar dbcontext
    public DbSet<User> Users { get; set; }
    public DbSet<Venues> Venues { get; set; }
    public DbSet<User_Venue> UserVenues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    }
}

