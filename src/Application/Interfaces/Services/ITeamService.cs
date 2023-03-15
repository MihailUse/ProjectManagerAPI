using Application.DTO.Common;
using Application.DTO.Team;

namespace Application.Interfaces.Services;

public interface ITeamService
{
    Task<PaginatedList<TeamBriefDto>> GetList(Guid projectId, SearchTeamDto searchDto);
    Task<TeamDto> GetById(Guid projectId, Guid id);
    Task<Guid> Create(Guid projectId, CreateTeamDto createDto);
    Task Update(Guid projectId, Guid id, UpdateTeamDto updateDto);
    Task Delete(Guid projectId, Guid id);
}