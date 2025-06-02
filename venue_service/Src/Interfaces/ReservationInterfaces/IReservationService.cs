using venue_service.Src.Dtos.Payment;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Enums;

namespace venue_service.Src.Interfaces.ReservationInterfaces;

public interface IReservationService
{
    Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto);
    Task<ReservationPaymentResponseDto> PayReservationAsync(int reservationId, PaymentRequestDto dto);
    Task<ReservationPaymentStatusDto> GetPaymentStatusAsync(int reservationId);
    Task<ReservationResponseDto> GetNextUserReservationAsync(int userId);
    Task<ReservationsResponseDto> GetReservationsByUserIdAsync(int userId, ReservationStatusEnum? status);
}
