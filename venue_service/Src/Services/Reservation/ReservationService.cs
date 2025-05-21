using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.Payment;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Enums;
using venue_service.Src.Enums.Payment;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.Reservation;
using venue_service.Src.Interfaces.Venue;
using venue_service.Src.Models.Payment;

namespace venue_service.Src.Services.Reservation;

public class ReservationService : IReservationService
{
    private readonly ReservationContext _reservationContext;
    private readonly UserContext _userContext;
    private readonly VenueContext _venueContext;
    private readonly IVenueAvaliabilityTime _venueAvailabilityTimeService;
    private readonly IPaymentService _paymentService;

    public ReservationService(
        ReservationContext reservationContext,
        UserContext userContext,
        VenueContext venueContext,
        IPaymentService paymentService,
        IVenueAvaliabilityTime venueAvailabilityTimeService)
    {
        _reservationContext = reservationContext;
        _userContext = userContext;
        _venueContext = venueContext;
        _paymentService = paymentService;
        _venueAvailabilityTimeService = venueAvailabilityTimeService;
    }


    public async Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto)
    {
        try
        {
            var userExists = await _userContext.User.FindAsync(dto.UserId);
            var venueExists = await _venueContext.Venues.FindAsync(dto.VenueId);
            var availability = await _venueContext.VenueAvailabilities.FindAsync(dto.VenueAvailabilityTimeId);

            if (userExists is null)
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "User not found");

            if (venueExists is null)
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Venue not found");

            if (availability is null)
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Availability not found");

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
                (string status, string mercadoPagoId) = await _paymentService.CreatePaymentAsync(
                    dto.UserEmail,
                    dto.CardToken,
                    availability.Price,
                    $"Reserva #{reservation.Id}",
                    (PaymentMethodEnum)dto.PaymentMethodId);

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
        catch (Exception ex)
        {
            throw new HttpResponseException(
                HttpStatusCode.InternalServerError,
                "Unexpected error",
                ex.InnerException?.Message ?? ex.Message
            );
        }
    }

    public async Task<ReservationsResponseDto> GetReservationsByUserIdAsync(int userId)
    {
        try
        {
            var reservations = await _reservationContext.Reservations
                                              .Where(r => r.UserId == userId)
                                              .ToListAsync();

            if (reservations.Count == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "No reservations found for this user");
            }

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
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "Unexpected error", ex.Message);
        }


    }

    public async Task<ReservationPaymentResponseDto> PayReservationAsync(int reservationId, PaymentRequestDto dto)
    {
        var reservation = await _reservationContext.Reservations
            .Include(r => r.PaymentRecord)
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservation == null)
            throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Reservation not found");

        if (reservation.PaymentRecord != null)
            throw new HttpResponseException(HttpStatusCode.BadRequest, "Already Paid", "This reservation already has a payment record");

        var availability = await _venueContext.VenueAvailabilities
            .FirstOrDefaultAsync(a => a.Id == reservation.VenueAvailabilityTimeId);

        if (availability == null)
            throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Availability not found");

        var paymentMethodEnum = (PaymentMethodEnum)reservation.PaymentMethodId;

        var (status, mercadoPagoId) = await _paymentService.CreatePaymentAsync(
            dto.UserEmail,
            dto.CardToken,
            availability.Price,
            $"Reserva #{reservationId}",
            paymentMethodEnum);

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


}
