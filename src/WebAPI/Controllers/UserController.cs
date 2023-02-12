using Application.Common.DTO.Auth;
using Application.Common.DTO.User;
using Application.Common.Models;
using Application.UseCases.Commands.CreateUser;
using Application.UseCases.Queries.GetUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
public class UserController : ApiControllerBase
{
    // GET: api/<UserController>
    [HttpGet]
    public async Task<ActionResult<PaginatedList<UserBriefDto>>> Get([FromQuery] GetUsersQuery query) => await Mediator.Send(query);

    // GET api/<UserController>/5
    [HttpGet("{id}")]
    public string Get(int id) => "value";

    // POST api/<UserController>
    [HttpPost]
    [AllowAnonymous]
    public async Task<AccessTokensDto> Post([FromBody] CreateUserCommand command) => await Mediator.Send(command);

    // PUT api/<UserController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<UserController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
