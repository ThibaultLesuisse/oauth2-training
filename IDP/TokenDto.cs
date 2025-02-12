using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace IDP;

public class TokenDto
{
    [FromForm(Name = "client_id")]
    public string ClientId { get; set; }
    
    [FromForm(Name = "client_secret")]
    public string ClientSecret { get; set; }
    
    [FromForm(Name = "code")]
    public string AuthorizationCode { get; set; }
    
    [FromForm(Name = "grant_type")]
    public string GrantType { get; set; }
    
    [FromForm(Name = "state")]
    public string State { get; set; }
}