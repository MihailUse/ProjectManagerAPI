using Application.DTO.Common;
using Application.DTO.MemberShip;

namespace Application.Interfaces.Services;

public interface IMemberShipService
{
    Task<List<Guid>> GetAssignedMemberShipIds(Guid projectId, List<Guid> userIds);
    Task<PaginatedList<MemberShipDto>> GetList(Guid projectId, SearchMemberShipDto searchDto);
    Task<Guid> Create(Guid projectId, CreateMemberShipDto createDto);
    Task Update(Guid id, UpdateMemberShipDto updateDto);
    Task Delete(Guid id);
}