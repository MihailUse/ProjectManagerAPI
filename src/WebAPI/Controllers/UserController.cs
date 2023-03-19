using Application.DTO.Auth;
using Application.DTO.Common;
using Application.DTO.User;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<PaginatedList<UserBriefDto>> Get([FromQuery] SearchUserDto query)
    {
        return await _userService.GetList(query);
    }

    [HttpGet("{id:guid}")]
    public async Task<UserDto> Get(Guid id)
    {
        return await _userService.Get(id);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<AccessTokensDto> Post([FromBody] CreateUserDto query)
    {
        return await _userService.Create(query);
    }

    [HttpPut("{id:guid}")]
    public async Task Put(Guid id, [FromBody] UpdateUserDto query)
    {
        await _userService.Update(id, query);
    }

    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        await _userService.Delete(id);
    }
}