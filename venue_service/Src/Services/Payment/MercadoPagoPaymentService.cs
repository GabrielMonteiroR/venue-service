using MercadoPago.Config;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using System.Text.Json;
using venue_service.Src.Config;
using venue_service.Src.Dtos.Payment;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.PaymentInterfaces;
using venue_service.Src.Middlewares;

namespace venue_service.Src.Services.Payment;

public class MercadoPagoPaymentService : IPaymentService
{
    private readonly MercadoPagoApiConfig _mpConfig;

    public MercadoPagoPaymentService(IOptions<MercadoPagoApiConfig> config)
    {
        _mpConfig = config.Value;
        MercadoPagoConfig.AccessToken = _mpConfig.AccessToken;
    }

    public async Task<CardTokenResponseDto?> GetCardTokenAsync(CardTokenRequestDto dto)
    {
        try
        {
            using var httpClient = new HttpClient();

            var url = _mpConfig.CardTokenRequestEndpoint;

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CardTokenResponseDto>(responseContent);
        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "Error", $"Erro ao obter token do cartão: {ex.Message}");
        }
    }
}
