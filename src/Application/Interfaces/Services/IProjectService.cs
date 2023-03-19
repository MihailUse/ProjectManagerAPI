using Application.DTO.Common;
using Application.DTO.Project;
using Domain.Enums;

namespace Application.Interfaces.Services;

public interface IProjectService
{
    Task<ProjectDto> Get(Guid id);
    Task<PaginatedList<ProjectBriefDto>> GetList(SearchProjectDto searchDto);
    Task<Guid> Create(CreateProjectDto createDto);
    Task Update(Guid id, UpdateProjectDto updateDto);
    Task Delete(Guid id);
    Task CheckPermission(Guid projectId, Role role);
    Task CheckProjectExists(Guid projectId);
}