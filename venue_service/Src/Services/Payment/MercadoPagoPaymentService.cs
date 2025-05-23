using MercadoPago.Client.Payment;
using MercadoPago.Config;
using venue_service.Src.Dtos.Payment;
using venue_service.Src.Enums;
using venue_service.Src.Enums.Payment;
using venue_service.Src.Interfaces.PaymentInterfaces;

namespace venue_service.Src.Services.Payment
{
    public class MercadoPagoPaymentService : IPaymentService
    {
        public MercadoPagoPaymentService(IConfiguration config)
        {
            MercadoPagoConfig.AccessToken = config["MercadoPago:AccessToken"];
        }

        public async Task<(string status, string mercadoPagoId, string? errorMessage)> CreatePaymentAsync(
            string email,
            string cardToken,
            decimal amount,
            string description,
            PaymentMethodEnum paymentMethod,
            long sellerUserId)
        {
            try
            {
                var methodId = paymentMethod.ToMercadoPagoId();

                var paymentRequest = new PaymentCreateRequest
                {
                    TransactionAmount = amount,
                    Token = cardToken,
                    Description = description,
                    Installments = 1,
                    PaymentMethodId = methodId,
                    Payer = new PaymentPayerRequest
                    {
                        Email = email
                    },
                    AdditionalInfo = new PaymentAdditionalInfoRequest
                    {
                        Items = new List<PaymentItemRequest>
                        {
                            new PaymentItemRequest
                            {
                                Title = description,
                                Quantity = 1,
                                UnitPrice = amount
                            }
                        }
                    },
                    SponsorId = sellerUserId
                };

                var client = new PaymentClient();
                var result = await client.CreateAsync(paymentRequest);

                return (result.Status.ToString(), result.Id.ToString(), null);
            }
            catch (Exception ex)
            {
                return ("error", string.Empty, ex.Message);
            }
        }

        public async Task<ReservationPaymentStatusDto> GetPaymentStatusAsync(int reservationId)
        {
            throw new NotImplementedException();
        }
    }
}
