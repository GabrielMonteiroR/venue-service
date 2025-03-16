using venue_service.Src.Dtos;
using venue_service.Src.Dtos.Reservation;

namespace venue_service.Src.Services
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto);
        Task<IEnumerable<ReservationResponseDto>> GetReservationsByUserIdAsync(int userId);
    }
}
