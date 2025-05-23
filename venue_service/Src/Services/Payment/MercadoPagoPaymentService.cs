using MercadoPago.Client.Payment;
using MercadoPago.Config;
using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.Payment;
using venue_service.Src.Enums;
using venue_service.Src.Enums.Payment;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.PaymentInterfaces;

namespace venue_service.Src.Services.Payment
{
    public class MercadoPagoPaymentService : IPaymentService
    {
        private readonly ReservationContext _reservationContext;

        public MercadoPagoPaymentService(IConfiguration config, ReservationContext reservationContext)
        {
            MercadoPagoConfig.AccessToken = config["MercadoPago:AccessToken"];
            _reservationContext = reservationContext;
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
                Payer = new PaymentPayerRequest { Email = email },
                ApplicationFee = amount * 0.3m,
                Metadata = new Dictionary<string, string>
                {
                    {"collector_id", receiverId  }
                }
            };

            var client = new PaymentClient();
            var result = await client.CreateAsync(paymentRequest);

            return (result.Status.ToString(), result.Id.ToString());
        }

        public async Task<ReservationPaymentStatusDto> GetPaymentStatusAsync(int reservationId)
        {
            try
            {
                var reservation = await _reservationContext.Reservations
                    .Include(r => r.PaymentRecord)
                    .FirstOrDefaultAsync(r => r.Id == reservationId);

                if (reservation == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Reservation not found");

                var record = reservation.PaymentRecord;

                return new ReservationPaymentStatusDto
                {
                    ReservationId = reservation.Id,
                    HasPayment = record != null,
                    PaymentStatus = record?.Status,
                    PaidAt = record?.PaidAt
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving payment status.", ex.Message);
            }
        }
    }
}