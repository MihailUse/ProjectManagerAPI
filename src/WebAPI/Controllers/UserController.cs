using Application.Common.DTO;
using Application.Common.Models;
using Application.UseCases.User.Queries.GetUsersWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class UserController : ApiControllerBase
{
    // GET: api/<UserController>
    [HttpGet]
    public async Task<ActionResult<PaginatedList<UserBriefDto>>> Get([FromQuery] GetUsersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    // GET api/<UserController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<UserController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

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
