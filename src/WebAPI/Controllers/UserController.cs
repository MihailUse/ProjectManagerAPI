using Application.DTO.Auth;
using Application.DTO.User;
using Application.Interfaces.Services;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<UserBriefDto>>> Get([FromQuery] GetUsersDto query)
    {
        return await _userService.GetUsers(query);
    }

    [HttpGet("{id}")]
    public string Get(int id) => "value";

    [AllowAnonymous]
    [HttpPost]
    public async Task<AccessTokensDto> Post([FromBody] CreateUserDto query)
    {
        return await _userService.CreateUser(query);
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}