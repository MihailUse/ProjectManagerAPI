namespace API.Models.Auth;

public class RequestTokenModel
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}