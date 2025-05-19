using venue_service.Src.Dtos.Reservation;

namespace venue_service.Src.Iterfaces.Reservation
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto, int userId);
        Task<ReservationsResponseDto> GetReservationsByUserIdAsync(int userId);
    }
}
