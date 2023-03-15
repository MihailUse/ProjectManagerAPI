using Application.DTO.Auth;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IIdentityService
{
    AccessTokensDto GenerateTokens(UserSession userSession);
    Task<AccessTokensDto> AuthorizeAsync(string login, string password);
    Task<AccessTokensDto> ReAuthorizeAsync(string refreshToken);
    Task<bool> IsValidSession(Guid sessionId);
}