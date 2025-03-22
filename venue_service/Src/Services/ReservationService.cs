using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos;
using venue_service.Src.Enums;
using venue_service.Src.Exceptions;
using venue_service.Src.Models;

namespace venue_service.Src.Services;

public class ReservationService : IReservationService
{
    private readonly DatabaseContext _context;

    public ReservationService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
        var venueExists = await _context.Venues.AnyAsync(v => v.Id == dto.VenueId);
        var availabilityExists = await _context.LocationAvailabilityTimes.AnyAsync(l => l.Id == dto.LocationAvailabilityTimeId);
        var paymentMethodExists = await _context.PaymentMethods.AnyAsync(pm => pm.Id == dto.PaymentMethodId);

        if (!userExists)
            throw new HttpResponseException(HttpStatusCode.BadRequest, "Validation Error", "User does not exist");

        if (!venueExists)
            throw new HttpResponseException(HttpStatusCode.BadRequest, "Validation Error", "Venue does not exist");

        if (!availabilityExists)
            throw new HttpResponseException(HttpStatusCode.BadRequest, "Validation Error", "Availability not found");

        if (!paymentMethodExists)
            throw new HttpResponseException(HttpStatusCode.BadRequest, "Validation Error", "Payment Method invalid");

        try
        {
            var reservation = new Reservation
            {
                UserId = dto.UserId,
                VenueId = dto.VenueId,
                LocationAvailabilityTimeId = dto.LocationAvailabilityTimeId,
                PaymentMethodId = dto.PaymentMethodId,
                Status = StatusEnum.PENDING.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return new ReservationResponseDto
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                VenueId = reservation.VenueId,
                LocationAvailabilityTimeId = reservation.LocationAvailabilityTimeId,
                PaymentMethodId = reservation.PaymentMethodId,
                Status = reservation.Status,
                CreatedAt = reservation.CreatedAt
            };
        }
        catch (Exception e)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", e.Message);
        }
    }

    public async Task<IEnumerable<ReservationResponseDto>> GetReservationsByUserIdAsync(int userId)
    {
        try
        {
            var reservations = await _context.Reservations.Where(r => r.UserId == userId).Select(r => new ReservationResponseDto
            {
                Id = r.Id,
                UserId = r.UserId,
                VenueId = r.VenueId,
                LocationAvailabilityTimeId = r.LocationAvailabilityTimeId,
                PaymentMethodId = r.PaymentMethodId,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            }).ToListAsync();

            if (reservations.Count == 0)
                throw new HttpResponseException(HttpStatusCode.NoContent, "No Content", $"There's no reservations for user with id {userId}");

            return reservations;
        }
        catch (Exception e)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", e.Message);
        }
    }
}
