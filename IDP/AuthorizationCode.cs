namespace IDP;

public class AuthorizationCode
{
    public string Code { get; set; }
    public User User { get; set; }
    public string State { get; set; }
}