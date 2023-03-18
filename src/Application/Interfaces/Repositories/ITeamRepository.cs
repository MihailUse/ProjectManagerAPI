using Application.DTO.Common;
using Application.DTO.Team;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface ITeamRepository
{
    Task<Team?> FindById(Guid id);
    Task<List<Team>> GetListByIds(Guid projectId, List<Guid> teamIds);
    Task<PaginatedList<TeamDto>> GetList(Guid projectId, SearchTeamDto searchDto);
    Task Add(Team team);
    Task Update(Team team);
    Task Remove(Team team);
}