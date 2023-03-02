using Application.DTO.Common;
using Application.DTO.MemberShip;

namespace Application.Interfaces.Services;

public interface IMemberShipService
{
    Task<PaginatedList<MemberShipDto>> GetList(SearchMemberShipDto searchDto);
    Task<Guid> Create(CreateMemberShipDto createDto);
    Task Update(Guid id, UpdateMemberShipDto updateDto);
    Task Delete(Guid id);
}