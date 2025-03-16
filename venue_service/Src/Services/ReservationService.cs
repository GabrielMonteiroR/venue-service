using Microsoft.EntityFrameworkCore;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos;
using venue_service.Src.Dtos.Reservation;

namespace venue_service.Src.Services
{
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
            var VenueExists = await _context.Venues.AnyAsync(v => v.Id == dto.VenueId);
            var availabilityExists = await _context.LocationAvailabilityTimes.AnyAsync(l => l.Id == dto.LocationAvailabilityTimeId);
            var paymentMethodExists = await _context.PaymentMethods.AnyAsync(pm => pm.Id == dto.PaymentMethodId);

            if (!userExists) throw new Exception("User not found") { Data = { { "ErrorHttpNumber", 404 } } };
        }
    }
}
