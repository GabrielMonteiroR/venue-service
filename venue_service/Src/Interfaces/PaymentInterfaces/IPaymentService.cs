using venue_service.Src.Dtos.Payment;
using venue_service.Src.Enums.Payment;

namespace venue_service.Src.Interfaces.PaymentInterfaces
{
    public interface IPaymentService
    {
        Task<CardTokenResponseDto?> GetCardTokenAsync(CardTokenRequestDto dto);
    }
}
