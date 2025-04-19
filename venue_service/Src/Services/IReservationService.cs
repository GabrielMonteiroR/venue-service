using venue_service.Src.Dtos;

namespace venue_service.Src.Services
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto, int userId);
        Task<ReservationsResponseDto> GetReservationsByUserIdAsync(int userId);
        //Task<ReservationResponseDto> UpdateReservationAsync(int id, UpdateReservationDto dto);
        //Task<bool> DeleteReservationAsync(int id);
    }
}
