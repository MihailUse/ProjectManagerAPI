using Application.DTO.Common;
using Application.DTO.Task;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<PaginatedList<TaskBriefDto>> Get([FromQuery] SearchTaskDto query)
    {
        return await _taskService.GetList(query);
    }

    [HttpGet("{id:guid}")]
    public async Task<TaskDto> Get(Guid id)
    {
        return await _taskService.GetById(id);
    }

    [HttpPost]
    public async Task<Guid> Post([FromBody] CreateTaskDto query)
    {
        return await _taskService.Create(query);
    }

    [HttpPut("{id:guid}")]
    public async Task Put(Guid id, [FromBody] UpdateTaskDto query)
    {
        await _taskService.Update(id, query);
    }

    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        await _taskService.Delete(id);
    }

    [HttpPost("{id:guid}/Assignee")]
    public async Task SetAssignees(Guid id, [FromBody] SetAssigneesDto query)
    {
        await _taskService.SetAssignees(id, query);
    }
}