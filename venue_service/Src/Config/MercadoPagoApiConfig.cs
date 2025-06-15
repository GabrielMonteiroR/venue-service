using System.Text.Json.Serialization;

namespace venue_service.Src.Config;

public class MercadoPagoApiConfig
{
    [JsonPropertyName("AccessToken")]
    public string AccessToken { get; set; }

    [JsonPropertyName("PublicKey")]
    public string PublicKey { get; set; }

    [JsonPropertyName("ReceiverId")]
    public string ReceiverId { get; set; }

    [JsonPropertyName("CardTokenRequestEndpoint")]
    public string CardTokenRequestEndpoint { get; set; }
}
