using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IDP.Pages;

public class IndexModel : PageModel
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public string Username { get; set; }
    
    [BindProperty(SupportsGet = true)]
    [Required]
    public string Password { get; set; }
    
    public IActionResult OnGet()
    {
        var clientId = Request.Query["client_id"];
        var redirectUri = Request.Query["redirect_uri"];
        var state = Request.Query["state"];
        var responseType = Request.Query["response_type"];
        
        ClientDatabase.Clients.TryGetValue(clientId, out var client);

        if (client is null)
        {
            return BadRequest();
        }

        if (!client.RedirectUris.Contains(redirectUri))
        {
            return BadRequest();
        }

        if (responseType != "code")
        {
            return BadRequest();
        }

        TempData["state"] = state;
        
        ViewData["Title"] = "IDP";
        
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        UserDatabase.Users.TryGetValue(Username, out var user);

        if (user is null)
        {
            ModelState.AddModelError("Username", "Something was wrong");
            return Page();
        }

        if (user.Password != Password)
        {
            ModelState.AddModelError("Username", "Something was wrong");
            return Page();
        }

        var authorizationCode = Guid.NewGuid().ToString();
        
        AuthorizationCodeDatabase.AuthorizationCodes.Add(new AuthorizationCode { Code = authorizationCode, State = Request.Query["state"]! , User = user! });
        
        return Redirect(Request.Query["redirect_uri"] + $"?code={authorizationCode}&state={Request.Query["state"]}");
    }
}