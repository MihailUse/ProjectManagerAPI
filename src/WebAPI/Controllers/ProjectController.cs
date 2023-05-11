using Application.DTO.Common;
using Application.DTO.Project;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("Api/Project")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<PaginatedList<ProjectBriefDto>> Get([FromQuery] SearchProjectDto query)
    {
        return await _projectService.GetList(query);
    }

    [HttpGet("{projectId:guid}")]
    public async Task<ProjectDto> Get(Guid id)
    {
        return await _projectService.Get(id);
    }

    [HttpPost]
    public async Task<Guid> Post([FromBody] CreateProjectDto query)
    {
        return await _projectService.Create(query);
    }

    [HttpPut("{projectId:guid}")]
    [CheckPermission(Role.Administrator)]
    public async Task Put(Guid projectId, [FromBody] UpdateProjectDto query)
    {
        await _projectService.Update(projectId, query);
    }

    [HttpDelete("{projectId:guid}")]
    [CheckPermission(Role.Owner)]
    public async Task Delete(Guid projectId)
    {
        await _projectService.Delete(projectId);
    }
}