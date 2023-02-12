namespace Application.Common.DTO.Auth;

public class AccessTokensDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
