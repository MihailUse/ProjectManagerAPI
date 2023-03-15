using Application.DTO.Common;
using Application.DTO.Status;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface IStatusRepository
{
    Task<Status?> FindById(Guid id);
    Task<Status?> FindByName(string name, Guid? projectId);
    Task<PaginatedList<StatusDto>> GetList(Guid projectId, SearchStatusDto searchDto);
    Task Add(Status status);
    Task Update(Status status);
    Task Remove(Status status);
    Task<bool> CheckExists(Guid id);
    Task<bool> CheckExists(string name, Guid? projectId);
}