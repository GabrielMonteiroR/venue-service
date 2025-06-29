using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Dtos.Reservation.ByUserId;
using venue_service.Src.Dtos.Streak;

namespace venue_service.Src.Interfaces.ReservationInterfaces;

public interface IReservationService
{
    Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto);
    Task<ReservationsResponseByIdDto> GetReservationsByUserIdAsync(int userId);
    Task<ReservationsResponseDto> GetReservationByVenueIdAsync(int venueId);
    Task<bool> PayReservationAsync(int reservationId);
    Task<ReservationsResponseByIdDto> GetHistoryByUserIdAsync(int userId);
    Task<ReservationsResponseByIdDto> GetNextUserReservationAsync(int userId);
    Task<ReservationsResponseDto> GetHistoryByVenueId(int VenueId);
    Task<StreakDto> GetUserStreak(int userId);
}
