using venue_service.Src.Dtos.Payment;
using venue_service.Src.Enums.Payment;

namespace venue_service.Src.Interfaces.PaymentInterfaces
{
    public interface IPaymentService
    {
        Task<(string status, string mercadoPagoId, string? errorMessage)> CreatePaymentAsync(
            string email,
            string cardToken,
            decimal amount,
            string description,
            PaymentMethodEnum paymentMethod,
            long sellerUserId);

        Task<ReservationPaymentStatusDto> GetPaymentStatusAsync(int reservationId);
    }
}
