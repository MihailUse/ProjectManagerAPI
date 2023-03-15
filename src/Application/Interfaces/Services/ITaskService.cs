using Application.DTO.Common;
using Application.DTO.Task;

namespace Application.Interfaces.Services;

public interface ITaskService
{
    Task<TaskDto> GetById(Guid id);
    Task<PaginatedList<TaskBriefDto>> GetList(SearchTaskDto searchDto);
    Task<Guid> Create(Guid projectId, CreateTaskDto createDto);
    Task Update(Guid id, UpdateTaskDto updateDto);
    Task Delete(Guid id);
    Task SetAssignees(Guid id, SetAssigneesDto setAssigneesDto);
}