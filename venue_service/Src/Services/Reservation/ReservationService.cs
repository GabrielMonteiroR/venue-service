using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.AvailabilityTimes;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Dtos.Reservation.ByUserId;
using venue_service.Src.Dtos.Streak;
using venue_service.Src.Dtos.User;
using venue_service.Src.Dtos.Venue;
using venue_service.Src.Enums;
using venue_service.Src.Enums.Payment;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.AvailableTimesInterfaces;
using venue_service.Src.Interfaces.PaymentInterfaces;
using venue_service.Src.Interfaces.ReservationInterfaces;

namespace venue_service.Src.Services.Reservation;

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

    public async Task<ReservationsResponseByIdDto> GetReservationsByUserIdAsync(int userId)
    {
        try
        {
            var user = await _userContext.Users.FindAsync(userId);
            if (user is null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with ID {userId} does not exist.");
            }

            var reservations = await _reservationContext.Reservations
            .Include(r => r.VenueAvailabilityTime)
            .Include(r => r.Venue)
            .ThenInclude(v => v.VenueImages)
            .Include(r => r.Venue)
            .ThenInclude(v => v.Owner)
            .Include(r => r.User)
            .Where(r => r.UserId == userId)
            .Where(r => r.VenueAvailabilityTime.EndDate >= DateTime.UtcNow)
            .OrderBy(r => r.VenueAvailabilityTime.StartDate)
            .ToListAsync();


            if (reservations.IsNullOrEmpty())
            {
                return new ReservationsResponseByIdDto
                {
                    Message = $"No reservations found for user with id {userId}.",
                    Reservations = new List<ReservationResponseByIdDto>()
                };
            }

            return new ReservationsResponseByIdDto
            {
                Message = $"Reservations found for user with id {userId}.",
                Reservations = reservations.Select(r => new ReservationResponseByIdDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    VenueId = r.VenueId,
                    PaymentMethodId = r.PaymentMethodId,
                    IsPaid = r.IsPaid,
                    Locator = new PartialUserResponseDto
                    {
                        Email = r.Venue.Owner.Email,
                        FirstName = r.Venue.Owner.FirstName,
                        LastName = r.Venue.Owner.LastName,
                        Phone = r.Venue.Owner.Phone,
                        ProfileImage = r.Venue.Owner.ProfileImageUrl
                    },
                    VenueAvailabilityTime = new VenueAvailabilityTimeDto
                    {
                        Id = r.VenueAvailabilityTime.Id,
                        StartDate = r.VenueAvailabilityTime.StartDate,
                        EndDate = r.VenueAvailabilityTime.EndDate,
                        VenueId = r.VenueAvailabilityTime.VenueId,
                        IsReserved = r.VenueAvailabilityTime.IsReserved,
                        Price = r.VenueAvailabilityTime.Price
                    },
                    VenueAvailabilityTimeId = r.VenueAvailabilityTimeId,
                    Venue = new VenueResponseDto
                    {
                        Id = r.Venue.Id,
                        Name = r.Venue.Name,
                        Capacity = r.Venue.Capacity,
                        City = r.Venue.City,
                        Complement = r.Venue.Complement,
                        Description = r.Venue.Description,
                        ImageUrls = r.Venue.VenueImages.Select(img => img.ImageUrl).ToList(),
                        State = r.Venue.State,
                        Latitude = r.Venue.Latitude,
                        Longitude = r.Venue.Longitude,
                        OwnerId = r.Venue.OwnerId,
                        OwnerName = r.Venue.Owner.FirstName + r.Venue.Owner.LastName,
                        Number = r.Venue.Number,
                        Neighborhood = r.Venue.Neighborhood,
                        PostalCode = r.Venue.PostalCode,
                        Rules = r.Venue.Rules,
                        Street = r.Venue.Street,
                        VenueTypeId = r.Venue.VenueTypeId,
                    },
                }).ToList()
            };

        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving the reservations.", ex.Message);
        }
    }

    public async Task<ReservationsResponseByIdDto> GetNextUserReservationAsync(int userId)
    {
        try
        {
            var reservation = await _reservationContext.Reservations
     .Include(r => r.VenueAvailabilityTime)
     .Include(r => r.User)
     .Include(r => r.PaymentMethod)
     .Include(r => r.Venue)
         .ThenInclude(v => v.Owner)
     .Include(r => r.Venue)
         .ThenInclude(v => v.VenueImages)
     .Where(r => r.UserId == userId)
     .Where(r => r.VenueAvailabilityTime.EndDate >= DateTime.UtcNow)
     .OrderBy(r => r.VenueAvailabilityTime.StartDate)
     .FirstOrDefaultAsync(); ;
            if (reservation is null)
            {
                return new ReservationsResponseByIdDto
                {
                    Message = $"No upcoming reservations found for user with id {userId}.",
                    Reservations = new List<ReservationResponseByIdDto>()
                };
            }
            return new ReservationsResponseByIdDto
            {
                Message = $"Upcoming reservation found for user with id {userId}.",
                Reservations = new List<ReservationResponseByIdDto>
                {
                    new ReservationResponseByIdDto
                    {
                        Id = reservation.Id,
                        UserId = reservation.UserId,
                        VenueId = reservation.VenueId,
                        PaymentMethodId = reservation.PaymentMethodId,
                        IsPaid = reservation.IsPaid,
                        VenueAvailabilityTime = new VenueAvailabilityTimeDto
                        {
                            Id = reservation.VenueAvailabilityTime.Id,
                            StartDate = reservation.VenueAvailabilityTime.StartDate,
                            EndDate = reservation.VenueAvailabilityTime.EndDate,
                            Price = reservation.VenueAvailabilityTime.Price,
                            VenueId = reservation.VenueAvailabilityTime.VenueId,
                            IsReserved = reservation.VenueAvailabilityTime.IsReserved,
                        },
                        VenueAvailabilityTimeId = reservation.VenueAvailabilityTimeId,
                        Locator = new PartialUserResponseDto
                        {
                            Email = reservation.User.Email,
                            FirstName = reservation.User.FirstName,
                            LastName = reservation.User.LastName,
                            Phone = reservation.User.Phone,
                            ProfileImage = reservation.User.ProfileImageUrl
                        },
                        Venue = new VenueResponseDto
                        {
                            Id = reservation.Venue.Id,
                            Name = reservation.Venue.Name,
                            Capacity = reservation.Venue.Capacity,
                            City = reservation.Venue.City,
                            Complement = reservation.Venue.Complement,
                            Description = reservation.Venue.Description,
                            ImageUrls = reservation.Venue.VenueImages.Select(img => img.ImageUrl).ToList(),
                            State = reservation.Venue.State,
                            Latitude = reservation.Venue.Latitude,
                            Longitude = reservation.Venue.Longitude,
                            OwnerId = reservation.Venue.OwnerId,
                            OwnerName = $"{reservation.Venue.Owner.FirstName} {reservation.Venue.Owner.LastName}",
                            Number = reservation.Venue.Number,
                            Neighborhood = reservation.Venue.Neighborhood,
                            PostalCode = reservation.Venue.PostalCode,
                            Rules = reservation.Venue.Rules,
                            Street = reservation.Venue.Street,
                            VenueTypeId = reservation.Venue.VenueTypeId
                        }
                    }
                }
            };
        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving the next user reservation.", ex.Message);
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

    public async Task<ReservationsResponseByIdDto> GetHistoryByUserIdAsync(int userId)
    {
        try
        {
            var reservations = await _reservationContext.Reservations
                .Include(r => r.VenueAvailabilityTime)
                .Include(r => r.Venue)
                .ThenInclude(v => v.VenueImages)
                .Include(r => r.Venue)
                .ThenInclude(v => v.Owner)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .Where(r => r.VenueAvailabilityTime.EndDate < DateTime.UtcNow)
                .OrderByDescending(r => r.VenueAvailabilityTime.EndDate)
                .ToListAsync();

            if (reservations.IsNullOrEmpty())
            {
                return new ReservationsResponseByIdDto
                {
                    Message = $"No reservation history found for user with id {userId}.",
                    Reservations = new List<ReservationResponseByIdDto>()
                };
            }

            return new ReservationsResponseByIdDto
            {
                Message = $"Reservation history found for user with id {userId}.",
                Reservations = reservations.Select(r => new ReservationResponseByIdDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    VenueId = r.VenueId,
                    PaymentMethodId = r.PaymentMethodId,
                    IsPaid = r.IsPaid,
                    Venue = new VenueResponseDto
                    {
                        Id = r.Venue.Id,
                        Name = r.Venue.Name,
                        Capacity = r.Venue.Capacity,
                        City = r.Venue.City,
                        Complement = r.Venue.Complement,
                        Description = r.Venue.Description,
                        ImageUrls = r.Venue.VenueImages.Select(img => img.ImageUrl).ToList(),
                        State = r.Venue.State,
                        Latitude = r.Venue.Latitude,
                        Longitude = r.Venue.Longitude,
                        OwnerId = r.Venue.OwnerId,
                        OwnerName = $"{r.Venue.Owner.FirstName} {r.Venue.Owner.LastName}",
                        Number = r.Venue.Number,
                        Neighborhood = r.Venue.Neighborhood,
                        PostalCode = r.Venue.PostalCode,
                        Rules = r.Venue.Rules,
                        Street = r.Venue.Street,
                        VenueTypeId = r.Venue.VenueTypeId
                    },
                    VenueAvailabilityTime = new VenueAvailabilityTimeDto
                    {
                        Id = r.VenueAvailabilityTime.Id,
                        StartDate = r.VenueAvailabilityTime.StartDate,
                        EndDate = r.VenueAvailabilityTime.EndDate,
                        VenueId = r.VenueAvailabilityTime.VenueId,
                        IsReserved = r.VenueAvailabilityTime.IsReserved,
                        Price = r.VenueAvailabilityTime.Price
                    },
                    VenueAvailabilityTimeId = r.VenueAvailabilityTimeId,
                    Locator = new PartialUserResponseDto
                    {
                        Email = r.Venue.Owner.Email,
                        FirstName = r.Venue.Owner.FirstName,
                        LastName = r.Venue.Owner.LastName,
                        Phone = r.Venue.Owner.Phone,
                        ProfileImage = r.Venue.Owner.ProfileImageUrl

                    }
                }).ToList()
            };

        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving the reservation history.", ex.Message);
        }
    }

    public async Task<ReservationsResponseDto> GetHistoryByVenueId(int VenueId)
    {
        try
        {
            var reservations = await _reservationContext.Reservations
                .Include(r => r.VenueAvailabilityTime)
                .Include(u => u.User)
                .Include(p => p.PaymentMethod)
                .Where(v => v.VenueId == VenueId)
                .Where(vt => vt.VenueAvailabilityTime.EndDate < DateTime.UtcNow)
                .ToListAsync();

            if (reservations.IsNullOrEmpty())
            {
                return new ReservationsResponseDto
                {
                    Message = $"No reservation history found for venue with id {VenueId}.",
                    Reservations = new List<ReservationResponseDto>()
                };
            }
            return new ReservationsResponseDto
            {
                Message = $"Reservation history found for venue with id {VenueId}.",
                Reservations = reservations.Select(r => new ReservationResponseDto
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
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving the reservation history.", ex.Message);
        }
    }

    public async Task<StreakDto> GetUserStreak(int userId)
    {
        try
        {
            var dates = await _reservationContext.Reservations
                .Include(r => r.VenueAvailabilityTime)
                .Where(r => r.UserId == userId && r.IsPaid)
                .Select(r => r.VenueAvailabilityTime.StartDate.Date)
                .Distinct()
                .ToListAsync();

            if (!dates.Any())
            {
                return new StreakDto
                {
                    UserId = userId,
                    StreakCount = 0,
                    Message = "Usuário ainda não possui reservas pagas."
                };
            }

            var calendar = CultureInfo.InvariantCulture.Calendar;

            var weeks = dates
                .Select(date => new
                {
                    Year = calendar.GetYear(date),
                    Week = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
                })
                .Distinct()
                .OrderByDescending(w => w.Year)
                .ThenByDescending(w => w.Week)
                .ToList();

            int streak = 1;
            for (int i = 1; i < weeks.Count; i++)
            {
                var prev = weeks[i - 1];
                var curr = weeks[i];

                if ((prev.Year == curr.Year && prev.Week == curr.Week + 1) ||
                    (prev.Year == curr.Year + 1 && prev.Week == 1 && curr.Week == GetLastWeekOfYear(curr.Year)))
                {
                    streak++;
                }
                else
                {
                    break;
                }
            }

            return new StreakDto
            {
                UserId = userId,
                StreakCount = streak,
                Message = streak > 1
                    ? $"Usuário está há {streak} semanas consecutivas praticando!"
                    : "Usuário praticou apenas uma semana até agora. Vamos manter o ritmo!"
            };
        }
        catch (Exception ex)
        {
            throw new HttpResponseException(
                HttpStatusCode.InternalServerError,
                "Erro ao calcular o streak do usuário.",
                ex.Message);
        }
    }

    private int GetLastWeekOfYear(int year)
    {
        var lastDay = new DateTime(year, 12, 31);
        var calendar = CultureInfo.InvariantCulture.Calendar;
        return calendar.GetWeekOfYear(lastDay, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }


}