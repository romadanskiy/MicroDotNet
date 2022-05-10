using System.Text.Json.Serialization;

namespace AuthorizationServer.Web.Tests;

public class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
}