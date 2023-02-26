using Application.DTO.Common;
using Application.DTO.Project;
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

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<PaginatedList<ProjectBriefDto>> Get([FromQuery] GetProjectsDto query)
    {
        return await _projectService.GetProjects(query);
    }

    [HttpGet("{id:guid}")]
    public async Task<ProjectDto> Get(Guid id)
    {
        return await _projectService.GetProject(id);
    }

    [HttpPost]
    public async Task<Guid> Post([FromBody] CreateProjectDto query)
    {
        return await _projectService.CreateProject(query);
    }

    [HttpPut("{id:guid}")]
    public async void Put(Guid id, [FromBody] UpdateProjectDto query)
    {
        await _projectService.UpdateProject(id, query);
    }

    [HttpDelete("{id:guid}")]
    public async void Delete(Guid id)
    {
        await _projectService.DeleteProject(id);
    }
}