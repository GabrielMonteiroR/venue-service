using venue_service.Src.Dtos.Payment;
using venue_service.Src.Dtos.Reservation;

namespace venue_service.Src.Interfaces.Reservation
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto);
        Task<ReservationsResponseDto> GetReservationsByUserIdAsync(int userId);
        Task<ReservationPaymentResponseDto> PayReservationAsync(int reservationId, PaymentRequestDto dto);
        Task<ReservationPaymentStatusDto> GetPaymentStatusAsync(int reservationId);
    }
}
