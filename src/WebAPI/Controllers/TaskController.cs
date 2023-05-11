using Application.DTO.Common;
using Application.DTO.Task;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers;

[CheckPermission(Role.MemberShip)]
[Authorize]
[ApiController]
[Route("Api/Project/{projectId:guid}/Task")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("{id:guid}")]
    public async Task<TaskDto> Get(Guid id)
    {
        return await _taskService.GetById(id);
    }

    [HttpGet]
    public async Task<PaginatedList<TaskBriefDto>> Get(Guid projectId, [FromQuery] SearchTaskDto query)
    {
        return await _taskService.GetList(query);
    }

    [HttpPost]
    public async Task<Guid> Post(Guid projectId, [FromBody] CreateTaskDto query)
    {
        return await _taskService.Create(projectId, query);
    }

    [HttpPut("{id:guid}")]
    public async Task Put(Guid id, Guid projectId, [FromBody] UpdateTaskDto query)
    {
        await _taskService.Update(id, query);
    }

    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id, Guid projectId)
    {
        await _taskService.Delete(id);
    }

    [HttpPut("{id:guid}/Assignee")]
    public async Task SetAssignees(Guid id, Guid projectId, [FromBody] SetAssigneesDto query)
    {
        await _taskService.SetAssignees(id, query);
    }

    [HttpPut("{id:guid}/AssigneeTeam")]
    public async Task SetAssigneeTeams(Guid id, Guid projectId, [FromBody] SetAssigneeTeamsDto query)
    {
        await _taskService.SetAssigneeTeams(id, query);
    }
}