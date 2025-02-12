using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using IDP;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.MapPost("/token", Results<Ok<TokenResponse>, BadRequest> (
    [FromForm(Name = "client_id")] string ClientId,
    [FromForm(Name = "client_secret")] string ClientSecret,
    [FromForm(Name = "grant_type")] string GrantType,
    [FromForm(Name = "code")] string Code,
    HttpContext httpContext) =>
{
    var authorizationCode = AuthorizationCodeDatabase
        .AuthorizationCodes
        .FirstOrDefault(x => x.Code == Code);

    if (authorizationCode is null)
    {
        return TypedResults.BadRequest();
    }

    var accessToken = new TokenResponse()
    {
        AccessToken = Guid.NewGuid().ToString(),
        ExpiresIn = DateTime.Now.AddHours(1).Subtract(DateTime.UnixEpoch).TotalSeconds,
    };

    return TypedResults.Ok(new TokenResponse() { AccessToken = Guid.NewGuid().ToString() });
}).DisableAntiforgery();
app.UseAuthorization();
app.MapStaticAssets();


app.MapRazorPages()
    .WithStaticAssets();

app.Run();