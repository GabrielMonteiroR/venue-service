using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.Payment;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Enums;
using venue_service.Src.Enums.Payment;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.PaymentInterfaces;
using venue_service.Src.Interfaces.ReservationInterfaces;
using venue_service.Src.Interfaces.VenueInterfaces;
using venue_service.Src.Models.Payment;

namespace venue_service.Src.Services.Reservation
{
    public class ReservationService : IReservationService
    {
        private readonly ReservationContext _reservationContext;
        private readonly UserContext _userContext;
        private readonly VenueContext _venueContext;
        private readonly IVenueAvailabilityTime _venueAvailabilityTimeService;
        private readonly IPaymentService _paymentService;

        public ReservationService(
            ReservationContext reservationContext,
            UserContext userContext,
            VenueContext venueContext,
            IPaymentService paymentService,
            IVenueAvailabilityTime venueAvailabilityTimeService)
        {
            _reservationContext = reservationContext;
            _userContext = userContext;
            _venueContext = venueContext;
            _paymentService = paymentService;
            _venueAvailabilityTimeService = venueAvailabilityTimeService;
        }

        public async Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto)
        {
            var user = await _userContext.User.FindAsync(dto.UserId)
                ?? throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "User not found");

            var venue = await _venueContext.Venues.FindAsync(dto.VenueId)
                ?? throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Venue not found");

            var availability = await _venueContext.VenueAvailabilities.FindAsync(dto.VenueAvailabilityTimeId)
                ?? throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Availability not found");

            var isAvailable = await _venueAvailabilityTimeService.IsThisTimeAvailableToBook(dto.VenueAvailabilityTimeId);
            if (!isAvailable)
                throw new HttpResponseException(HttpStatusCode.Conflict, "Conflict", "This time is not available");

            if (!Enum.IsDefined(typeof(PaymentMethodEnum), dto.PaymentMethodId))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Validation Error", "Invalid payment method");

            var reservation = new ReservationEntity
            {
                UserId = dto.UserId,
                VenueId = dto.VenueId,
                VenueAvailabilityTimeId = dto.VenueAvailabilityTimeId,
                PaymentMethodId = dto.PaymentMethodId,
                Status = (int)ReservationStatusEnum.PENDING,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _reservationContext.Reservations.Add(reservation);
            await _reservationContext.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(dto.CardToken) && !string.IsNullOrWhiteSpace(dto.UserEmail))
            {
                var owner = await _userContext.User.FindAsync(venue.OwnerId);
                if (owner == null || string.IsNullOrWhiteSpace(owner.MercadoPagoUserId))
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "Payment Error", "Locador não possui conta MercadoPago cadastrada");

                long receiverId = long.Parse(owner.MercadoPagoUserId);

                (string status, string mercadoPagoId, string? error) = await _paymentService.CreatePaymentAsync(
                    dto.UserEmail,
                    dto.CardToken,
                    availability.Price,
                    $"Reserva #{reservation.Id}",
                    (PaymentMethodEnum)dto.PaymentMethodId,
                    receiverId);

                var paymentRecord = new PaymentRecordEntity
                {
                    ReservationId = reservation.Id,
                    Amount = availability.Price,
                    Status = status,
                    MercadoPagoPaymentId = mercadoPagoId,
                    CreatedAt = DateTime.UtcNow,
                    PaidAt = status == "approved" ? DateTime.UtcNow : null
                };

                _reservationContext.PaymentRecords.Add(paymentRecord);
                await _reservationContext.SaveChangesAsync();
            }

            return new ReservationResponseDto
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                VenueId = reservation.VenueId,
                PaymentMethodId = reservation.PaymentMethodId,
                Status = reservation.Status,
                CreatedAt = reservation.CreatedAt
            };
        }

        public async Task<ReservationsResponseDto> GetReservationsByUserIdAsync(int userId)
        {
            var reservations = await _reservationContext.Reservations
                .Where(r => r.UserId == userId)
                .ToListAsync();

            if (!reservations.Any())
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "No reservations found for this user");

            return new ReservationsResponseDto
            {
                Message = "Reservations found",
                Reservations = reservations.Select(r => new ReservationResponseDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    VenueId = r.VenueId,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                }).ToList()
            };
        }

        public async Task<ReservationPaymentResponseDto> PayReservationAsync(int reservationId, PaymentRequestDto dto)
        {
            var reservation = await _reservationContext.Reservations
                .Include(r => r.PaymentRecord)
                .Include(r => r.Venue)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Reservation not found");

            if (reservation.PaymentRecord != null)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Already Paid", "This reservation already has a payment record");

            var availability = await _venueContext.VenueAvailabilities
                .FirstOrDefaultAsync(a => a.Id == reservation.VenueAvailabilityTimeId);

            if (availability == null)
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Availability not found");

            var owner = await _userContext.User.FindAsync(reservation.Venue.OwnerId);
            if (owner == null || string.IsNullOrWhiteSpace(owner.MercadoPagoUserId))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Payment Error", "Locador não possui conta MercadoPago cadastrada");

            long receiverId = long.Parse(owner.MercadoPagoUserId);
            var paymentMethodEnum = (PaymentMethodEnum)reservation.PaymentMethodId;

            (string status, string mercadoPagoId, string? _) = await _paymentService.CreatePaymentAsync(
                dto.UserEmail,
                dto.CardToken,
                availability.Price,
                $"Reserva #{reservationId}",
                paymentMethodEnum,
                receiverId);

            var record = new PaymentRecordEntity
            {
                ReservationId = reservationId,
                Amount = availability.Price,
                Status = status,
                MercadoPagoPaymentId = mercadoPagoId,
                CreatedAt = DateTime.UtcNow,
                PaidAt = status == "approved" ? DateTime.UtcNow : null
            };

            _reservationContext.PaymentRecords.Add(record);
            await _reservationContext.SaveChangesAsync();

            return new ReservationPaymentResponseDto
            {
                ReservationId = reservationId,
                Paid = status == "approved",
                PaymentStatus = status,
                PaidAt = record.PaidAt
            };
        }

        public async Task<ReservationPaymentStatusDto> GetPaymentStatusAsync(int reservationId)
        {
            var payment = await _reservationContext.PaymentRecords
                .FirstOrDefaultAsync(p => p.ReservationId == reservationId);

            if (payment == null)
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "No payment found for this reservation");

            return new ReservationPaymentStatusDto
            {
                ReservationId = reservationId,
                PaymentStatus = payment.Status,
                PaidAt = payment.PaidAt,
                HasPayment = true
            };
        }
    }
}
