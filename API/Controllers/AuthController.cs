using API.Models.Auth;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<TokenModel> GetTokens(RequestTokenModel requestToken)
    {
        return await _authService.GetTokens(requestToken.Login, requestToken.Password);
    }

    [HttpPost]
    public async Task<TokenModel> GetTokensByRefeshToken(RequestRefreshTokenModel requestToken)
    {
        return await _authService.GetTokensByRefreshToken(requestToken.RefreshToken);
    }
}