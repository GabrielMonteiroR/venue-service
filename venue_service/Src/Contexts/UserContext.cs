
using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models.User;
using venue_service.Src.Models.Venue;

namespace venue_service.Src.Contexts;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }

    public DbSet<UserEntity> User => Set<UserEntity>();
    public DbSet<RoleEntity> Role => Set<RoleEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<Venue_SportEntity>();


        modelBuilder.Entity<UserEntity>().ToTable("users");
        modelBuilder.Entity<RoleEntity>().ToTable("roles");

        modelBuilder.Entity<RoleEntity>()
            .HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
