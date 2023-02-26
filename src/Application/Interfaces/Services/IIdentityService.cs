using Application.DTO.Auth;

namespace Application.Interfaces.Services;

public interface IIdentityService
{
    Task<AccessTokensDto> CreateUserAsync(string login, string password);
    Task<AccessTokensDto> AuthorizeAsync(string login, string password);
    Task<AccessTokensDto> ReAuthorizeAsync(string refreshToken);
    Task<bool> IsValidSession(Guid sessionId);
}