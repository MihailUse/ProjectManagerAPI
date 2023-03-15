using Application.DTO.Auth;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    public async Task<AccessTokensDto> Get([FromBody] GetAccessTokensDto getAccessTokensDto)
    {
        return await _identityService.AuthorizeAsync(getAccessTokensDto.Login, getAccessTokensDto.Password);
    }

    [HttpPut]
    public async Task<AccessTokensDto> Put([FromBody] string refreshToken)
    {
        return await _identityService.ReAuthorizeAsync(refreshToken);
    }
}