using Application.DTO.Common;
using Application.DTO.Team;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("Api/Project/{projectId:guid}/Team")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    [CheckPermission(Role.MemberShip)]
    public async Task<PaginatedList<TeamDto>> Get(Guid projectId, [FromQuery] SearchTeamDto query)
    {
        return await _teamService.GetList(projectId, query);
    }

    [HttpPost("{id:guid}")]
    [CheckPermission(Role.Administrator)]
    public async Task<Guid> Post(Guid projectId, [FromBody] CreateTeamDto query)
    {
        return await _teamService.Create(projectId, query);
    }

    [HttpPut("{id:guid}")]
    [CheckPermission(Role.Administrator)]
    public async Task Put(Guid id, [FromBody] UpdateTeamDto query)
    {
        await _teamService.Update(id, query);
    }

    [HttpDelete("{id:guid}")]
    [CheckPermission(Role.Administrator)]
    public async Task Delete(Guid id)
    {
        await _teamService.Delete(id);
    }
}