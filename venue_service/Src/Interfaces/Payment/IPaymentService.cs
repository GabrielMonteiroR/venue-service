
using venue_service.Src.Dtos.Payment;
using venue_service.Src.Enums.Payment;

public interface IPaymentService
{
    Task<(string status, string mercadoPagoId)> CreatePaymentAsync(
        string email,
        string cardToken,
        decimal amount,
        string description,
        PaymentMethodEnum paymentMethod);

    Task<ReservationPaymentStatusDto> GetPaymentStatusAsync(int reservationId);

}
