using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos;
using venue_service.Src.Enums;
using venue_service.Src.Exceptions;
using venue_service.Src.Models;
using venue_service.Src.Services;

namespace Src.Services;

public class ReservationService : IReservationService
{
    private readonly DatabaseContext _context;

    public ReservationService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto, int userId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        var venueExists = await _context.Venues.AnyAsync(v => v.Id == dto.VenueId);
        var availabilityExists = await _context.VenueAvailabilities.AnyAsync(lat => lat.Id == dto.VenueAvailabilityTimeId);
        var paymentMethodExists = await _context.PaymentMethods.AnyAsync(pm => pm.Id == dto.PaymentMethodId);

        if (!userExists) throw new HttpResponseException(HttpStatusCode.BadRequest, "Validation Error", "User does not exist");
        if (!venueExists) throw new HttpResponseException(HttpStatusCode.BadRequest, "Validation Error", "Venue does not exist");
        if (!availabilityExists) throw new HttpResponseException(HttpStatusCode.BadRequest, "Validation Error", "Availability not found");
        if (!paymentMethodExists) throw new HttpResponseException(HttpStatusCode.BadRequest, "Validation Error", "Payment Method invalid");

        var reservation = new Reservation
        {
            UserId = userId,
            VenueId = dto.VenueId,
            VenueAvailabilityTimeId = dto.VenueAvailabilityTimeId,
            PaymentMethodId = dto.PaymentMethodId,
            Status = ReservationStatusEnum.PENDING.ToString(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return new ReservationResponseDto
        {
            CreatedAt = reservation.CreatedAt,
            Id = reservation.Id,
            Status = reservation.Status,
            UserId = reservation.UserId,
            VenueId = reservation.VenueId,
        };
    }

    public async Task<ReservationsResponseDto> GetReservationsByUserIdAsync(int userId)
    {
        try
        {
            var reservations = await _context.Reservations
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

//    public async Task<ReservationResponseDto> UpdateReservationAsync(int id, UpdateReservationDto dto)
//{
//    var reservation = await _context.Reservations
//                                    .Include(r => r.User)
//                                    .Include(r => r.Venue)
//                                    .FirstOrDefaultAsync(r => r.Id == id);

//    if (reservation == null)
//    {
//        throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Reservation not found");
//    }

//    reservation.Status = dto.Status;
//    reservation.VenueId = dto.VenueId;
//    reservation.VenueAvailabilityTimeId = dto.VenueAvailabilityTimeId;
//    reservation.PaymentMethodId = dto.PaymentMethodId;
//    reservation.UpdatedAt = DateTime.UtcNow;

//    _context.Reservations.Update(reservation);
//    await _context.SaveChangesAsync();

//    return _mapper.Map<ReservationResponseDto>(reservation);
//}

//public async Task<bool> DeleteReservationAsync(int id)
//{
//    var reservation = await _context.Reservations.FindAsync(id);

//    if (reservation == null)
//    {
//        throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Reservation not found");
//    }

//    _context.Reservations.Remove(reservation);
//    await _context.SaveChangesAsync();

//    return true;
//}
}
