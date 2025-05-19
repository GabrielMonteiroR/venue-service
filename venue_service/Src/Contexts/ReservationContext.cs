
using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models;

namespace venue_service.Src.Contexts;

public class ReservationContext : DbContext
{
    public ReservationContext(DbContextOptions<ReservationContext> options) : base(options) { }

    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Venue_Sport>();
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Reservation>().ToTable("reservations");
        modelBuilder.Entity<PaymentMethod>().ToTable("payment_methods");

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
    }
}
