using Application.DTO.Common;
using Application.DTO.Project;

namespace Application.Interfaces.Services;

public interface IProjectService
{
    Task<ProjectDto> GetById(Guid id);
    Task<PaginatedList<ProjectBriefDto>> GetList(SearchProjectDto searchDto);
    Task<Guid> Create(CreateProjectDto createDto);
    Task Update(Guid id, UpdateProjectDto updateDto);
    Task Delete(Guid id);
}