using Application.DTO.Common;
using Application.DTO.Project;
using Application.DTO.Team;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ITeamService _teamService;

    public ProjectController(IProjectService projectService, ITeamService teamService)
    {
        _projectService = projectService;
        _teamService = teamService;
    }

    [HttpGet]
    public async Task<PaginatedList<ProjectBriefDto>> Get([FromQuery] SearchProjectDto query)
    {
        return await _projectService.GetList(query);
    }

    [HttpGet("{id:guid}")]
    public async Task<ProjectDto> Get(Guid id)
    {
        return await _projectService.GetById(id);
    }

    [HttpPost]
    public async Task<Guid> Post([FromBody] CreateProjectDto query)
    {
        return await _projectService.Create(query);
    }

    [HttpPut("{id:guid}")]
    public async Task Put(Guid id, [FromBody] UpdateProjectDto query)
    {
        await _projectService.Update(id, query);
    }

    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        await _projectService.Delete(id);
    }

    [HttpGet("{projectId:guid}")]
    public async Task<PaginatedList<TeamBriefDto>> Get([FromRoute] Guid projectId, [FromQuery] SearchTeamDto query)
    {
        return await _teamService.GetList(projectId, query);
    }

    [HttpGet("{projectId:guid}/{id:guid}")]
    public async Task<TeamDto> Get([FromRoute] Guid projectId, Guid id)
    {
        return await _teamService.GetById(projectId, id);
    }

    [HttpPost("{projectId:guid}/{id:guid}")]
    public async Task<Guid> Post([FromRoute] Guid projectId, [FromBody] CreateTeamDto query)
    {
        return await _teamService.Create(projectId, query);
    }

    [HttpPut("{projectId:guid}/{id:guid}")]
    public async Task Put([FromRoute] Guid projectId, [FromRoute] Guid id, [FromBody] UpdateTeamDto query)
    {
        await _teamService.Update(projectId, id, query);
    }

    [HttpDelete("{projectId:guid}/{id:guid}")]
    public async Task Delete([FromRoute] Guid projectId, [FromRoute] Guid id)
    {
        await _teamService.Delete(projectId, id);
    }
}