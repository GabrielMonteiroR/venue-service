using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Dtos.Reservation.ByUserId;

namespace venue_service.Src.Interfaces.ReservationInterfaces;

public interface IReservationService
{
    Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto);
    Task<ReservationsResponseByIdDto> GetReservationsByUserIdAsync(int userId);
    Task<ReservationResponseDto> GetNextUserReservationAsync(int userId);
    Task<ReservationsResponseDto> GetReservationByVenueIdAsync(int venueId);
    Task<bool> PayReservationAsync(int reservationId);
    Task<ReservationsResponseDto> GetHistoryByUserIdAsync(int userId);
    Task<ReservationsResponseDto> GetHistoryByVenueId(int VenueId);
}
