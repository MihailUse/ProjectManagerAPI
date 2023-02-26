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
    public async Task<PaginatedList<UserBriefDto>> Get([FromQuery] GetUsersDto query)
    {
        return await _userService.GetUsers(query);
    }

    [HttpGet("{id:guid}")]
    public async Task<UserDto> Get(Guid id)
    {
        return await _userService.GetUser(id);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<AccessTokensDto> Post([FromBody] CreateUserDto query)
    {
        return await _userService.CreateUser(query);
    }

    [HttpPut("{id:guid}")]
    public async void Put(Guid id, [FromBody] UpdateUserDto query)
    {
        await _userService.UpdateUser(id, query);
    }

    [HttpDelete("{id:guid}")]
    public async void Delete(Guid id)
    {
        await _userService.DeleteUser(id);
    }
}