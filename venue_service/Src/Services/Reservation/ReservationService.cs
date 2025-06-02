using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.Payment;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Dtos.User;
using venue_service.Src.Enums;
using venue_service.Src.Enums.Payment;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.PaymentInterfaces;
using venue_service.Src.Interfaces.ReservationInterfaces;
using venue_service.Src.Models.Payment;

namespace venue_service.Src.Services.Reservation
{
    public class ReservationService : IReservationService
    {
        private readonly ReservationContext _reservationContext;
        private readonly UserContext _userContext;
        private readonly VenueContext _venueContext;
        private readonly IPaymentService _paymentService;

        public ReservationService(
            ReservationContext reservationContext,
            UserContext userContext,
            VenueContext venueContext,
            IPaymentService paymentService
        )
        {
            _reservationContext = reservationContext;
            _userContext = userContext;
            _venueContext = venueContext;
            _paymentService = paymentService;
        }

        public async Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto)
        {
            try
            {
                var user = await _userContext.Users
                    .Include(u => u.Reservations)
                    .FirstOrDefaultAsync(u => u.Id == dto.UserId);
                if (user is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with ID {dto.UserId} does not exist.");

                var venue = await _venueContext.Venues
                    .Include(v => v.Owner)
                    .FirstOrDefaultAsync(v => v.Id == dto.VenueId);
                if (venue is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Venue not found", $"Venue with ID {dto.VenueId} does not exist.");

                var newReservation = new ReservationEntity
                {
                    UserId = dto.UserId,
                    VenueId = dto.VenueId,
                    ScheduleId = dto.ScheduleId,
                    TotalAmount = dto.TotalAmount,
                    PaymentMethodId = dto.PaymentMethodId,
                    Status = (int)ReservationStatusEnum.PENDING,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _reservationContext.Reservations.Add(newReservation);
                await _reservationContext.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(dto.CardToken) && !string.IsNullOrWhiteSpace(dto.UserEmail))
                {
                    var owner = venue.Owner;
                    if (owner == null || string.IsNullOrWhiteSpace(owner.MercadoPagoUserId))
                        throw new HttpResponseException(HttpStatusCode.BadRequest, "Payment Error", "Locador não possui conta MercadoPago cadastrada");

                    long receiverId = long.Parse(owner.MercadoPagoUserId);

                    (string status, string mercadoPagoId, string? error) = await _paymentService.CreatePaymentAsync(
                        dto.UserEmail,
                        dto.CardToken,
                        dto.TotalAmount,
                        $"Reserva #{newReservation.Id}",
                        (PaymentMethodEnum)dto.PaymentMethodId,
                        receiverId
                    );

                    var paymentRecord = new PaymentRecordEntity
                    {
                        ReservationId = newReservation.Id,
                        Amount = dto.TotalAmount,
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
                    Id = newReservation.Id,
                    UserId = newReservation.UserId,
                    VenueId = newReservation.VenueId,
                    PaymentMethodId = newReservation.PaymentMethodId,
                    Status = newReservation.Status,
                    CreatedAt = newReservation.CreatedAt
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while creating the reservation.", ex.Message);
            }
        }

        public async Task<ReservationsResponseDto> GetReservationsByUserIdAsync(int userId)
        {
            try
            {
                var user = await _userContext.Users.FindAsync(userId);
                if (user is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with ID {userId} does not exist.");

                var reservations = await _reservationContext.Reservations.Where(r => r.UserId == userId).ToListAsync();

                if (reservations is null || reservations.Count == 0)
                {
                    return new ReservationsResponseDto
                    {
                        Message = $"No reservations found for user with id {userId}.",
                        Reservations = new List<ReservationResponseDto>()
                    };
                }

                var content = reservations.Select(r => new ReservationResponseDto
                {
                    Id = r.Id,
                    UserId = user.Id,
                    VenueId = r.VenueId,
                    ScheduleId = r.ScheduleId,
                    PaymentMethodId = r.PaymentMethodId,
                    PaymentStatus = r.Status,
                    CreatedAt = r.CreatedAt
                }).ToList();

                return new ReservationsResponseDto
                {
                    Message = $"Reservations found for user with id {userId}.",
                    Reservations = content
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving reservations.", ex.Message);
            }
        }

        public async Task<ReservationPaymentResponseDto> PayReservationAsync(int reservationId, PaymentRequestDto dto)
        {
            var reservation = await _reservationContext.Reservations.FindAsync(reservationId);

            if (reservation == null)
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Reservation not found");

            if (reservation.PaymentRecord != null)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Already Paid", "This reservation already has a payment record");

            var owner = reservation.Venue.Owner;
            if (owner == null || string.IsNullOrWhiteSpace(owner.MercadoPagoUserId))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Payment Error", "Locador não possui conta MercadoPago cadastrada");

            long receiverId = long.Parse(owner.MercadoPagoUserId);
            var paymentMethodEnum = (PaymentMethodEnum)reservation.PaymentMethodId;

            (string status, string mercadoPagoId, string? _) = await _paymentService.CreatePaymentAsync(
                dto.UserEmail,
                dto.CardToken,
                reservation.TotalAmount,
                $"Reserva #{reservationId}",
                paymentMethodEnum,
                receiverId
            );

            var record = new PaymentRecordEntity
            {
                ReservationId = reservationId,
                Amount = reservation.TotalAmount,
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

        public async Task<ReservationResponseDto> GetNextUserReservationAsync(int userId)
        {
            try
            {
                var user = await _userContext.Users.FindAsync(userId);
                if (user is null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with ID {userId} does not exist.");
                }

                var nextReservation = await _reservationContext.Reservations
                    .Where(r => r.UserId == userId)
                    .FirstOrDefaultAsync(r => r.CreatedAt > DateTime.UtcNow);

                if (nextReservation is null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound, "No upcoming reservations", $"User with ID {userId} has no upcoming reservations.");
                }

                return new ReservationResponseDto
                {
                    Id = nextReservation.Id,
                    UserId = nextReservation.UserId,
                    VenueId = nextReservation.VenueId,
                    ScheduleId = nextReservation.ScheduleId,
                    TotalAmount = nextReservation.TotalAmount,
                    PaymentMethodId = nextReservation.PaymentMethodId,
                    Status = nextReservation.Status,
                    CreatedAt = nextReservation.CreatedAt
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving the last reservation.", ex.Message);
            }
        }
    }
}