using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.AvailabilityTimes;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Dtos.User;
using venue_service.Src.Enums;
using venue_service.Src.Enums.Payment;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.AvailableTimesInterfaces;
using venue_service.Src.Interfaces.PaymentInterfaces;
using venue_service.Src.Interfaces.ReservationInterfaces;

namespace venue_service.Src.Services.Reservation
{
    public class ReservationService : IReservationService
    {
        private readonly ReservationContext _reservationContext;
        private readonly UserContext _userContext;
        private readonly IAvailableTimesService _availableTimesService;
        private readonly VenueContext _venueContext;

        public ReservationService(
            ReservationContext reservationContext,
            UserContext userContext,
            VenueContext venueContext,
            IAvailableTimesService availableTimesService
        )
        {
            _reservationContext = reservationContext;
            _userContext = userContext;
            _venueContext = venueContext;
            _availableTimesService = availableTimesService;
        }

        public async Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto)
        {
            try
            {
                var user = await _userContext.Users.FindAsync(dto.UserId);
                if (user is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with ID {dto.UserId} does not exist.");
                var venue = await _venueContext.Venues.FindAsync(dto.VenueId);
                if (venue is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Venue not found", $"Venue with ID {dto.VenueId} does not exist.");

                var availableTime = await _venueContext.VenueAvailabilities.FindAsync(dto.VenueAvailabilityTimeId);

                if (availableTime is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Available time not found", $"No available time found for venue with ID {dto.VenueId} and time ID {dto.VenueAvailabilityTimeId}.");

                if (availableTime.IsReserved is true)
                {
                    throw new HttpResponseException(HttpStatusCode.Conflict, "Time slot already reserved", $"The time slot for venue with ID {dto.VenueId} is already reserved.");
                }

                var availableTimeIsFromThiVenue = availableTime.VenueId == dto.VenueId;

                if (!availableTimeIsFromThiVenue)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid venue availability time", $"The availability time with ID {dto.VenueAvailabilityTimeId} does not belong to the venue with ID {dto.VenueId}.");
                }
                
                var reservation = new ReservationEntity
                {
                    VenueId = dto.VenueId,
                    UserId = dto.UserId,
                    VenueAvailabilityTimeId = dto.VenueAvailabilityTimeId,
                    IsPaid = false,
                    PaymentMethodId = dto.PaymentMethodId
                };

                await _availableTimesService.SetTrueToIsReserved(dto.VenueAvailabilityTimeId);

                await _reservationContext.Reservations.AddAsync(reservation);
                await _reservationContext.SaveChangesAsync();

                return new ReservationResponseDto
                {
                    Id = reservation.Id,
                    UserId = reservation.UserId,
                    VenueId = reservation.VenueId,
                    PaymentMethodId = reservation.PaymentMethodId,
                    IsPaid = reservation.IsPaid,
                };
            }
            catch (DbUpdateException dbEx)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Database update error", dbEx.InnerException?.Message ?? dbEx.Message);
            }
            catch (HttpResponseException httpEx)
            {
                throw httpEx;
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
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with ID {userId} does not exist.");
                }

                var reservations = await _reservationContext.Reservations
                    .Include(vt => vt.VenueAvailabilityTime)
                    .Where(r => r.UserId == userId)
                    .Where(vt => vt.VenueAvailabilityTime.EndDate >= DateTime.UtcNow)
                    .OrderBy(r => r.VenueAvailabilityTime.StartDate)
                    .ToListAsync();

                if (reservations.IsNullOrEmpty())
                {
                    return new ReservationsResponseDto
                    {
                        Message = $"No reservations found for user with id {userId}.",
                        Reservations = new List<ReservationResponseDto>()
                    };
                }

                return new ReservationsResponseDto
                {
                    Message = $"Reservations found for user with id {userId}.",
                    Reservations = reservations.Select(r => new ReservationResponseDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        VenueId = r.VenueId,
                        PaymentMethodId = r.PaymentMethodId,
                        IsPaid = r.IsPaid,
                        VenueAvailabilityTime = new VenueAvailabilityTimeDto
                        {
                            StartDate = r.VenueAvailabilityTime.StartDate,
                            EndDate = r.VenueAvailabilityTime.EndDate,
                            VenueId = r.VenueAvailabilityTime.VenueId,
                            IsReserved = r.VenueAvailabilityTime.IsReserved,
                        },
                        VenueAvailabilityTimeId = r.VenueAvailabilityTimeId,
                    }).ToList()
                };

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving the reservations.", ex.Message);
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
                    .FirstOrDefaultAsync(r => r.VenueAvailabilityTime.StartDate > DateTime.UtcNow);

                if (nextReservation is null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound, "No upcoming reservations", $"User with ID {userId} has no upcoming reservations.");
                }

                return new ReservationResponseDto
                {
                    Id = nextReservation.Id,
                    UserId = nextReservation.UserId,
                    VenueId = nextReservation.VenueId,
                    PaymentMethodId = nextReservation.PaymentMethodId,
                    IsPaid = nextReservation.IsPaid,
                    VenueAvailabilityTime = new VenueAvailabilityTimeDto
                    {
                        StartDate = nextReservation.VenueAvailabilityTime.StartDate,
                        EndDate = nextReservation.VenueAvailabilityTime.EndDate,
                        VenueId = nextReservation.VenueAvailabilityTime.VenueId,
                        IsReserved = nextReservation.VenueAvailabilityTime.IsReserved,
                    },
                    VenueAvailabilityTimeId = nextReservation.VenueAvailabilityTimeId,
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
                    .Include(r => r.VenueAvailabilityTime)
                    .Include(u => u.User)
                    .Include(p => p.PaymentMethod)
                    .Where(v => v.VenueId == venueId)
                    .Where(vt => vt.VenueAvailabilityTime.EndDate >= DateTime.UtcNow)
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
                        PaymentMethodId = r.PaymentMethodId,
                        IsPaid = r.IsPaid,
                        VenueAvailabilityTime = new VenueAvailabilityTimeDto
                        {
                            Id = r.VenueAvailabilityTime.Id,
                            StartDate = r.VenueAvailabilityTime.StartDate,
                            EndDate = r.VenueAvailabilityTime.EndDate,
                            Price = r.VenueAvailabilityTime.Price,
                            VenueId = r.VenueAvailabilityTime.VenueId,
                            IsReserved = r.VenueAvailabilityTime.IsReserved,
                        },
                        VenueAvailabilityTimeId = r.VenueAvailabilityTimeId,
                        User = new PartialUserResponseDto
                        {
                            Email = r.User.Email,
                            FirstName = r.User.FirstName,
                            LastName = r.User.LastName,
                            Phone = r.User.Phone,
                            ProfileImage = r.User.ProfileImageUrl
                        }
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

                reservation.IsPaid = true;

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