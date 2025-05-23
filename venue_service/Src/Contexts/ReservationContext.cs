using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models.Payment;
using venue_service.Src.Models.Venue;

namespace venue_service.Src.Contexts
{
    public class ReservationContext : DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options) : base(options) { }

        public DbSet<ReservationEntity> Reservations => Set<ReservationEntity>();
        public DbSet<PaymentMethodEntity> PaymentMethods => Set<PaymentMethodEntity>();
        public DbSet<PaymentRecordEntity> PaymentRecords => Set<PaymentRecordEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Venue_SportEntity>();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReservationEntity>().ToTable("reservations");

            modelBuilder.Entity<PaymentMethodEntity>().ToTable("payment_methods");

            modelBuilder.Entity<ReservationEntity>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<ReservationEntity>()
                .HasOne(r => r.Venue)
                .WithMany()
                .HasForeignKey(r => r.VenueId);

            modelBuilder.Entity<ReservationEntity>()
                .HasOne(r => r.PaymentMethod)
                .WithMany(pm => pm.Reservations)
                .HasForeignKey(r => r.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentRecordEntity>().ToTable("payment_records");

            modelBuilder.Entity<PaymentRecordEntity>()
                .HasOne(pr => pr.Reservation)
                .WithOne(r => r.PaymentRecord)
                .HasForeignKey<PaymentRecordEntity>(pr => pr.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
