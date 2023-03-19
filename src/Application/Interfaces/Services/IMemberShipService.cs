using Application.DTO.Common;
using Application.DTO.MemberShip;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Services;

public interface IMemberShipService
{
    Task<PaginatedList<MemberShipDto>> GetList(Guid projectId, SearchMemberShipDto searchDto);
    Task<List<MemberShip>> GetListByIds(Guid projectId, List<Guid> memberShipIds);
    Task<Guid> Create(Guid projectId, CreateMemberShipDto createDto);
    Task Update(Guid id, UpdateMemberShipDto updateDto);
    Task Delete(Guid id);
}