using System.Text.Json.Serialization;

namespace IDP;

public class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("expires_in")]
    public double ExpiresIn { get; set; }
}