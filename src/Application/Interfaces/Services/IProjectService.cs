using Application.DTO.Common;
using Application.DTO.Project;

namespace Application.Interfaces.Services;

public interface IProjectService
{
    Task<ProjectDto> GetProject(Guid projectId);
    Task<PaginatedList<ProjectBriefDto>> GetProjects(GetProjectsDto getProjectsDto);
    Task<Guid> CreateProject(CreateProjectDto createProjectDto);
    Task UpdateProject(Guid projectId, UpdateProjectDto updateProjectDto);
    Task DeleteProject(Guid projectId);
}