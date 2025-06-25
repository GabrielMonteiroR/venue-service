using venue_service.Src.Dtos.Payment;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Enums;

namespace venue_service.Src.Interfaces.ReservationInterfaces;

public interface IReservationService
{
    Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto);
    Task<ReservationsResponseDto> GetReservationsByUserIdAsync(int userId);
    Task<ReservationResponseDto> GetNextUserReservationAsync(int userId);
    Task<ReservationsResponseDto> GetReservationByVenueIdAsync(int venueId);
    Task<bool> PayReservationAsync(int reservationId);
}
