using Application.DTO.Common;
using Application.DTO.Task;
using Task = System.Threading.Tasks.Task;
using TaskEntity = Domain.Entities.Task;

namespace Application.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<TaskEntity?> FindById(Guid id);
    Task<TaskDto?> FindByIdProjection(Guid id);
    Task<TaskEntity?> FindByIdWithMemberShip(Guid taskId, Guid memberShipId);
    Task<PaginatedList<TaskBriefDto>> GetList(SearchTaskDto searchDto);
    Task AddAsync(TaskEntity task);
    Task Update(TaskEntity task);
    Task Remove(TaskEntity task);
}