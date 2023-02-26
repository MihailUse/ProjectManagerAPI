using Application.DTO.Common;
using Application.DTO.Project;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Mappings;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _context;
    private readonly IDatabaseFunctions _databaseFunctions;
    private readonly ICurrentUserService _currentUserService;

    public ProjectService(
        IMapper mapper,
        IDatabaseContext context,
        IDatabaseFunctions databaseFunctions,
        ICurrentUserService currentUserService
    )
    {
        _mapper = mapper;
        _context = context;
        _databaseFunctions = databaseFunctions;
        _currentUserService = currentUserService;
    }

    public async Task<ProjectDto> GetProject(Guid projectId)
    {
        var project = await FindProject(projectId);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<PaginatedList<ProjectBriefDto>> GetProjects(GetProjectsDto getProjectsDto)
    {
        var query = string.IsNullOrEmpty(getProjectsDto.Search)
            ? _context.Projects
            : _context.Projects.Where(x => _databaseFunctions.ILike(x.Name, $"%{getProjectsDto.Search}%"));

        return await query.ProjectToPaginatedListAsync<ProjectBriefDto>(_mapper.ConfigurationProvider, getProjectsDto);
    }

    public async Task<Guid> CreateProject(CreateProjectDto createProjectDto)
    {
        var project = _mapper.Map<Project>(createProjectDto);
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        return project.Id;
    }

    public async Task UpdateProject(Guid projectId, UpdateProjectDto updateProjectDto)
    {
        var hasPermissions = await _context.MemberShips.AnyAsync(x
            => x.UserId == _currentUserService.UserId && x.Role.Name == Roles.Owner);
        if (!hasPermissions)
            throw new AccessDeniedException("No permission");

        var project = await FindProject(projectId);
        project = _mapper.Map(updateProjectDto, project);

        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProject(Guid projectId)
    {
        var isOwner = await _context.MemberShips.AnyAsync(x
            => x.UserId == _currentUserService.UserId && x.Role.Name == Roles.Owner);
        if (!isOwner)
            throw new AccessDeniedException("No permission");

        var project = await FindProject(projectId);
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    private async Task<Project> FindProject(Guid projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == default)
            throw new NotFoundException("Project not found");

        return project;
    }
}