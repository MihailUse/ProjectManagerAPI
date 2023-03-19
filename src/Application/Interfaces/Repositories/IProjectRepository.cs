using Application.DTO.Common;
using Application.DTO.Project;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<Project?> FindById(Guid id);
    Task<ProjectDto?> FindByIdProjection(Guid id);
    Task<Project?> FindByIdWithMembership(Guid id, Guid memberShipId);
    Task<PaginatedList<ProjectBriefDto>> GetListByMemberShip(SearchProjectDto searchDto, Guid memberShipId);
    Task Add(Project project);
    Task Update(Project project);
    Task Remove(Project project);
    Task<bool> CheckExists(Guid projectId);
}