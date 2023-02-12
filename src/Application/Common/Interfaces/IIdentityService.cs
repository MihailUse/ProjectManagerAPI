using Application.Common.DTO.Auth;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<AccessTokensDto> CreateUserAsync(string login, string password);
    Task<AccessTokensDto> AuthorizeAsync(string login, string password);
    Task<AccessTokensDto> ReAuthorizeAsync(Guid RefreshTokenId);
}
