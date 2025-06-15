using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using venue_service.Src.Config;

namespace venue_service.Src.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly HttpClient _http;
    private readonly MercadoPagoApiConfig _mercadoPagoApiConfig;

    public TestController(IOptions<MercadoPagoApiConfig> options)
    {
        _http = new HttpClient();
        _mercadoPagoApiConfig = options.Value;
    }

    [HttpPost("pay-fixed")]
    public async Task<IActionResult> SendFixedPayment()
    {
        try
        {
            var receiverId = "1415878307"; 

            var publicKey = _mercadoPagoApiConfig.PublicKey;
            var accessToken = _mercadoPagoApiConfig.AccessToken;

            var cardTokenRequest = new
            {
                card_number = "5031433215406351",
                expiration_month = 11,
                expiration_year = 2030,
                security_code = "123",
                cardholder = new
                {
                    name = "APRO",
                    identification = new
                    {
                        type = "CPF",
                        number = "19119119100"
                    }
                }
            };

            var cardJson = JsonSerializer.Serialize(cardTokenRequest);
            var tokenResponse = await _http.PostAsync(
                $"https://api.mercadopago.com/v1/card_tokens?public_key={publicKey}",
                new StringContent(cardJson, Encoding.UTF8, "application/json")
            );

            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
            if (!tokenResponse.IsSuccessStatusCode)
                return StatusCode(500, new { error = "Erro ao gerar token", detalhe = tokenContent });

            var tokenId = JsonDocument.Parse(tokenContent).RootElement.GetProperty("id").GetString();

            var paymentRequest = new
            {
                transaction_amount = 10,
                token = tokenId,
                description = "Teste direto",
                installments = 1,
                payment_method_id = "master",
                payer = new { email = "test_user_170454958@testuser.com" }, 
                sponsor_id = long.Parse(receiverId),
                additional_info = new
                {
                    items = new[]
                    {
                        new { title = "Agendamento", quantity = 1, unit_price = 10 }
                    }
                }
            };

            var paymentJson = JsonSerializer.Serialize(paymentRequest);

            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _http.DefaultRequestHeaders.Add("X-Idempotency-Key", Guid.NewGuid().ToString());

            var paymentResponse = await _http.PostAsync(
                "https://api.mercadopago.com/v1/payments",
                new StringContent(paymentJson, Encoding.UTF8, "application/json")
            );


            var paymentBody = await paymentResponse.Content.ReadAsStringAsync();
            if (!paymentResponse.IsSuccessStatusCode)
                return StatusCode(500, new { error = "Erro no pagamento", detalhe = paymentBody });

            return Ok(new
            {
                mensagem = "Pagamento testado com sucesso!",
                dados = JsonDocument.Parse(paymentBody).RootElement
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { erro = "Erro inesperado", detalhe = ex.Message });
        }
    }
}
