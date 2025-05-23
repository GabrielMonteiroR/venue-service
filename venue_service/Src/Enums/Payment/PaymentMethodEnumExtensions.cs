using venue_service.Src.Enums.Payment;

namespace venue_service.Src.Enums
{
    public static class PaymentMethodEnumExtensions
    {
        public static string ToMercadoPagoId(this PaymentMethodEnum method)
        {
            return method switch
            {
                PaymentMethodEnum.VISA => "visa",
                PaymentMethodEnum.MASTERCARD => "master",
                PaymentMethodEnum.PIX => "pix",
                _ => throw new ArgumentException("Método de pagamento não suportado")
            };
        }
    }
}
