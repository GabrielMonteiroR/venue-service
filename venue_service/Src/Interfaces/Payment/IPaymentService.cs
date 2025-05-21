namespace venue_service.Src.Interfaces.Payment
{
    public interface IPaymentService
    {
        Task<(string status, string mercadoPagoId)> CreatePaymentAsync(string email, string cardToken, decimal amount, string description);
    }
}
