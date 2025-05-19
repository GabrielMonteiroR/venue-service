using venue_service.Src.Dtos.Reservation;

namespace venue_service.Src.Services
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto, int userId);
        Task<ReservationsResponseDto> GetReservationsByUserIdAsync(int userId);
    }
}
