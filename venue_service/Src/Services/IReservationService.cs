using venue_service.Src.Dtos;

namespace venue_service.Src.Services
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto);
        Task<IEnumerable<ReservationResponseDto>> GetReservationsByUserIdAsync(int userId);
    }
}
