using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Enums;
using venue_service.Src.Enums.Payment;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.PaymentInterfaces;

namespace venue_service.Src.Services.Reservation
{
    public class ReservationService
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
                var user = await _userContext.Users.FindAsync(dto.UserId);
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
                    TotalAmount = dto.TotalAmount,
                    PaymentMethodId = dto.PaymentMethodId,
                    Status = (int)ReservationStatusEnum.PENDING,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _reservationContext.Reservations.Add(newReservation);
                await _reservationContext.SaveChangesAsync();

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

        public async Task<ReservationsResponseDto> GetReservationsByUserIdAsync(int userId, ReservationStatusEnum? status)
        {
            try
            {
                var user = await _userContext.Users.FindAsync(userId);
                if (user is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with ID {userId} does not exist.");

                var query = _reservationContext.Reservations
                    .Include(r => r.PaymentRecord)
                    .Where(r => r.UserId == userId);

                if (status.HasValue)
                {
                    query = query.Where(r => r.Status == (int)status.Value);
                }

                var reservations = await query.ToListAsync();

                if (reservations.Count == 0)
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
                    UserId = r.UserId,
                    VenueId = r.VenueId,
                    TotalAmount = r.TotalAmount,
                    PaymentMethodId = r.PaymentMethodId,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    PaidAt = r.PaymentRecord?.PaidAt,
                    IsPaid = r.PaymentRecord != null && r.PaymentRecord.Status == PaymentStatusEnum.APPROVED.ToString(),
                    PaymentStatus = r.PaymentRecord != null ? int.Parse(r.PaymentRecord.Status) : 0
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

        public async Task<ReservationsResponseDto> GetReservationByVenueIdAsync(int venueId)
        {
            try
            {
                var reservation = await _reservationContext.Reservations
                    .Include(r => r.PaymentRecord)
                    .Where(v => v.VenueId == venueId)
                    .ToListAsync();

                if (reservation.IsNullOrEmpty())
                {
                    return new ReservationsResponseDto
                    {
                        Message = $"No reservations found for venue with id {venueId}.",
                        Reservations = new List<ReservationResponseDto>()
                    };
                }

                return new ReservationsResponseDto
                {
                    Message = $"Reservations found for venue with id {venueId}.",
                    Reservations = reservation.Select(r => new ReservationResponseDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        VenueId = r.VenueId,
                        TotalAmount = r.TotalAmount,
                        PaymentMethodId = r.PaymentMethodId,
                        Status = r.Status,
                        CreatedAt = r.CreatedAt,
                        UpdatedAt = r.UpdatedAt,
                        PaidAt = r.PaymentRecord?.PaidAt,
                        IsPaid = r.PaymentRecord != null && r.PaymentRecord.Status == PaymentStatusEnum.APPROVED.ToString(),
                        PaymentStatus = r.PaymentRecord != null ? int.Parse(r.PaymentRecord.Status) : 0
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving the reservation.", ex.Message);
            }

        }

        public async Task<bool> PayReservationAsync(int reservationId)
        {
            try
            {
                var reservation = await _reservationContext.Reservations.FindAsync(reservationId);
                if (reservation is null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Reservation not found", $"Reservation with ID {reservationId} does not exist.");
                }

                reservation.Status = (int)ReservationStatusEnum.CONFIRMED;

                await _reservationContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while updating the reservation status.", ex.Message);
            }
        }
    }
}