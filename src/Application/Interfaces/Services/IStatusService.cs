using Application.DTO.Common;
using Application.DTO.Status;

namespace Application.Interfaces.Services;

public interface IStatusService
{
    Task<PaginatedList<StatusDto>> GetList(SearchStatusDto searchDto);
    Task<Guid> Create(CreateStatusDto createDto);
    Task Update(Guid id, UpdateStatusDto updateDto);
    Task Delete(Guid id);
}