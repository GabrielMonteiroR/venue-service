using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using Microsoft.Extensions.Configuration;
using venue_service.Src.Enums;
using venue_service.Src.Enums;
using venue_service.Src.Enums.Payment;
using venue_service.Src.Interfaces.Payment;

namespace venue_service.Src.Services.Payment
{
    public class MercadoPagoPaymentService : IPaymentService
    {
        public MercadoPagoPaymentService(IConfiguration config)
        {
            MercadoPagoConfig.AccessToken = config["MercadoPago:AccessToken"];
        }

        public async Task<(string status, string mercadoPagoId)> CreatePaymentAsync(
            string email,
            string cardToken,
            decimal amount,
            string description,
            PaymentMethodEnum paymentMethod)
        {
            var methodId = paymentMethod.ToMercadoPagoId();

            var paymentRequest = new PaymentCreateRequest
            {
                TransactionAmount = amount,
                Token = cardToken,
                Description = description,
                Installments = 1,
                PaymentMethodId = methodId,
                Payer = new PaymentPayerRequest { Email = email }
            };

            var client = new PaymentClient();
            var result = await client.CreateAsync(paymentRequest);

            return (result.Status.ToString(), result.Id.ToString());
        }
    }
}
