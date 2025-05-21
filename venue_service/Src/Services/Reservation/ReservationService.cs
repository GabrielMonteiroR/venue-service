using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Enums;
using venue_service.Src.Exceptions;
using venue_service.Src.Iterfaces.Reservation;
using venue_service.Src.Iterfaces.Venue;

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
            var availabilityExists = await _venueContext.VenueAvailabilities.FindAsync(dto.VenueAvailabilityTimeId);

            if(userExists is null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "User not found");
            }
            if(venueExists is null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Venue not found");
            }
            if (availabilityExists is null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Availability not found");
            }

            var isAvailable = await _venueAvailabilityTimeService.IsThisTimeAvailableToBook(dto.VenueAvailabilityTimeId);


        } catch(Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "Unexpected error", ex.Message);
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
        } catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "Unexpected error", ex.Message);
}


    }

}
