using Application.DTO.Common;
using Application.DTO.Task;
using TaskEntity = Domain.Entities.Task;

namespace Application.Interfaces.Services;

public interface ITaskService
{
    Task<TaskDto> GetById(Guid id);
    Task<PaginatedList<TaskBriefDto>> GetList(Guid projectId, SearchTaskDto searchDto);
    Task<Guid> Create(Guid projectId, CreateTaskDto createDto);
    Task Update(Guid id, UpdateTaskDto updateDto);
    Task Delete(Guid id);
    Task SetAssignees(Guid id, SetAssigneesDto setAssigneesDto);
    Task SetAssigneeTeams(Guid id, SetAssigneeTeamsDto setAssigneeTeamsDto);
    Task<TaskEntity> FindTask(Guid taskId);
}