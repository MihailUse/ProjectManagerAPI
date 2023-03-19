using Application.DTO.Common;
using Application.DTO.Status;

namespace Application.Interfaces.Services;

public interface IStatusService
{
    Task<PaginatedList<StatusDto>> GetList(Guid projectId, SearchStatusDto searchDto);
    Task<Guid> Create(Guid projectId, CreateStatusDto createDto);
    Task Update(Guid id, UpdateStatusDto updateDto);
    Task Delete(Guid id);
    Task CheckStatusExists(Guid statusId);
}