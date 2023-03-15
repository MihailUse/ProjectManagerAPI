using Application.DTO.Common;
using Application.DTO.Project;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IMapper _mapper;
    private readonly IProjectRepository _repository;
    private readonly IAttachService _attachService;
    private readonly Guid _currentUserId;

    public ProjectService(
        IMapper mapper,
        IProjectRepository repository,
        IAttachService attachService,
        ICurrentUserService currentUserService
    )
    {
        _mapper = mapper;
        _repository = repository;
        _attachService = attachService;
        _currentUserId = currentUserService.UserId;
    }

    public async Task<ProjectDto> Get(Guid id)
    {
        var project = await _repository.FindByIdProjection(id);
        if (project == default)
            throw new NotFoundException("Project not found");

        return project;
    }

    public async Task<PaginatedList<ProjectBriefDto>> GetList(SearchProjectDto searchDto)
    {
        return await _repository.GetListByMemberShip(searchDto, _currentUserId);
    }

    public async Task<Guid> Create(CreateProjectDto createDto)
    {
        var project = _mapper.Map<Project>(createDto);
        project.LogoId = await _attachService.GenerateImage();
        project.Memberships = new List<MemberShip>
        {
            new(_currentUserId, Role.Owner)
        };

        await _repository.Add(project);
        return project.Id;
    }

    public async Task Update(Guid id, UpdateProjectDto updateDto)
    {
        var project = await FindProject(id);
        if (project.LogoId != updateDto.LogoId)
            await _attachService.CheckAttachExists(updateDto.LogoId);

        project = _mapper.Map(updateDto, project);
        await _repository.Update(project);
    }

    public async Task Delete(Guid id)
    {
        var project = await FindProject(id);
        await _repository.Remove(project);
    }

    public async Task CheckPermission(Guid projectId, Role role)
    {
        var project = await _repository.FindByIdWithMembership(projectId, _currentUserId);
        if (project == default)
            throw new NotFoundException("Project not found");

        var currentMemberShip = project.Memberships.FirstOrDefault();
        if (currentMemberShip == default)
            throw new NotFoundException("Project not found");

        if (currentMemberShip.Role > role)
            throw new AccessDeniedException("No permission");
    }

    public async Task CheckProjectExists(Guid projectId)
    {
        var projectExists = await _repository.CheckExists(projectId);
        if (!projectExists)
            throw new NotFoundException("Project not found");
    }

    private async Task<Project> FindProject(Guid projectId)
    {
        var project = await _repository.FindById(projectId);
        if (project == default)
            throw new NotFoundException("Project not found");

        return project;
    }
}