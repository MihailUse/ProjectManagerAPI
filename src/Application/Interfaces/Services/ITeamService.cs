using Application.DTO.Common;
using Application.DTO.Team;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Services;

public interface ITeamService
{
    Task<List<Team>> GetListByIds(Guid projectId, List<Guid> teamIds);
    Task<PaginatedList<TeamDto>> GetList(Guid projectId, SearchTeamDto searchDto);
    Task<Guid> Create(Guid projectId, CreateTeamDto createDto);
    Task Update(Guid id, UpdateTeamDto updateDto);
    Task Delete(Guid id);
}