namespace IDP;

public class ClientDatabase
{
    public static Dictionary<string, Client> Clients { get; set; } = new Dictionary<string, Client>()
    {
        {
            "hogent", new Client()
            {
                ClientId = "hogent",
                ClientSecret = "secret",
                RedirectUris = ["http://localhost:5073/callback"],
            }
        }
    };
}