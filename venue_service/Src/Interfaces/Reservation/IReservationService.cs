using venue_service.Src.Dtos.Reservation;

namespace venue_service.Src.Interfaces.Reservation
{
    public interface IReservationService
    {
        Task<ReservationsResponseDto> GetReservationsByUserIdAsync(int userId);

    }
}
