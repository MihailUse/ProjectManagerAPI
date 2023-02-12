namespace Application.DTO.Auth;

public class GetAccessTokensDto
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}
