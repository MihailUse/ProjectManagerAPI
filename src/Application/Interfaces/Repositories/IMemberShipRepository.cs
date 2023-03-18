using Application.DTO.Common;
using Application.DTO.MemberShip;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface IMemberShipRepository
{
    Task<MemberShip?> FindById(Guid id);
    Task<List<MemberShip>> GetByIds(Guid projectId, List<Guid> memberShipIds);
    Task<PaginatedList<MemberShipDto>> GetList(Guid projectId, SearchMemberShipDto searchDto);
    Task Add(MemberShip memberShip);
    Task Update(MemberShip memberShip);
    Task Remove(MemberShip memberShip);
    Task<bool> CheckExists(Guid projectId, Guid userId);
}