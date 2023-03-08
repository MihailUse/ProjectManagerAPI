using Application.DTO.Common;
using Application.DTO.Project;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _database;
    private readonly IDatabaseFunctions _databaseFunctions;
    private readonly Guid _currentUserId;
    private readonly IAttachService _attachService;

    public ProjectService(
        IMapper mapper,
        IDatabaseContext database,
        IDatabaseFunctions databaseFunctions,
        ICurrentUserService currentUserService,
        IAttachService attachService
    )
    {
        _mapper = mapper;
        _database = database;
        _databaseFunctions = databaseFunctions;
        _attachService = attachService;
        _currentUserId = currentUserService.UserId;
    }

    public async Task<ProjectDto> GetById(Guid id)
    {
        await CheckPermission(id, Role.MemberShip);
        var project = await FindProject(id);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<PaginatedList<ProjectBriefDto>> GetList(SearchProjectDto searchDto)
    {
        var query = _database.Projects
            .Where(x => x.Memberships.Any(m => m.UserId == _currentUserId));

        if (!string.IsNullOrEmpty(searchDto.Search))
            query = query.Where(x => _databaseFunctions.ILike(x.Name, $"%{searchDto.Search}%"));

        return await query.ProjectToPaginatedListAsync<ProjectBriefDto>(_mapper.ConfigurationProvider, searchDto);
    }

    public async Task<Guid> Create(CreateProjectDto createDto)
    {
        var project = _mapper.Map<Project>(createDto);
        project.LogoId = await _attachService.GenerateImage();
        project.Memberships = new List<MemberShip>()
        {
            new MemberShip(_currentUserId, Role.Owner)
        };

        await _database.Projects.AddAsync(project);
        await _database.SaveChangesAsync();

        return project.Id;
    }

    public async Task Update(Guid id, UpdateProjectDto updateDto)
    {
        await CheckPermission(id, Role.Administrator);

        var project = await FindProject(id);
        if (project.LogoId != updateDto.LogoId)
        {
            var attachExists = await _database.Attaches.AnyAsync(x => x.Id == updateDto.LogoId);
            if (!attachExists)
                throw new NotFoundException("Attach not found");
        }

        project = _mapper.Map(updateDto, project);
        _database.Projects.Update(project);
        await _database.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        await CheckPermission(id, Role.Owner);

        var project = await FindProject(id);
        _database.Projects.Remove(project);
        await _database.SaveChangesAsync();
    }

    private async Task CheckPermission(Guid projectId, Role role)
    {
        var project = await _database.Projects
            .Include(x => x.Memberships.Where(m => m.UserId == _currentUserId))
            .FirstOrDefaultAsync(x => x.Id == projectId);
        if (project == default)
            throw new NotFoundException("Project not found");

        var currentMemberShip = project.Memberships.FirstOrDefault();
        if (currentMemberShip == default || currentMemberShip.Role > role)
            throw new AccessDeniedException("No permission");
    }

    private async Task<Project> FindProject(Guid projectId)
    {
        var project = await _database.Projects.FindAsync(projectId);
        if (project == default)
            throw new NotFoundException("Project not found");

        return project;
    }
}