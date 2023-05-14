using Application.DTO.Common;
using Application.DTO.Task;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;
using TaskEntity = Domain.Entities.Task;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly IMapper _mapper;
    private readonly ITaskRepository _repository;
    private readonly IStatusService _statusService;
    private readonly IMemberShipService _memberShipService;
    private readonly ITeamService _teamService;
    private readonly Guid _currentUserId;

    public TaskService(
        IMapper mapper,
        ITaskRepository repository,
        IStatusService statusService,
        IMemberShipService memberShipService,
        ICurrentUserService currentUserService,
        ITeamService teamService
    )
    {
        _mapper = mapper;
        _repository = repository;
        _statusService = statusService;
        _memberShipService = memberShipService;
        _teamService = teamService;
        _currentUserId = currentUserService.UserId;
    }

    public async Task<TaskDto> GetById(Guid id)
    {
        var task = await _repository.FindByIdProjection(id);
        if (task == default)
            throw new NotFoundException("Task not found");

        return task;
    }

    public async Task<PaginatedList<TaskBriefDto>> GetList(Guid projectId, SearchTaskDto searchDto)
    {
        return await _repository.GetList(projectId, searchDto);
    }

    public async Task<Guid> Create(Guid projectId, CreateTaskDto createDto)
    {
        await _statusService.CheckStatusExists(createDto.StatusId);

        var task = _mapper.Map<TaskEntity>(createDto);
        task.OwnerId = _currentUserId;
        task.ProjectId = projectId;

        await _repository.AddAsync(task);
        return task.Id;
    }

    public async Task Update(Guid id, UpdateTaskDto updateDto)
    {
        var task = await FindTask(id);

        if (task.OwnerId != _currentUserId)
            throw new AccessDeniedException("No permission");

        if (task.StatusId != updateDto.StatusId)
            await _statusService.CheckStatusExists(updateDto.StatusId);

        task = _mapper.Map(updateDto, task);
        await _repository.Update(task);
    }

    public async Task Delete(Guid id)
    {
        var task = await FindTask(id);

        if (task.OwnerId != _currentUserId)
            throw new AccessDeniedException("No permission");

        await _repository.Remove(task);
    }

    public async Task SetAssignees(Guid id, SetAssigneesDto setAssigneesDto)
    {
        var task = await FindTask(id);
        var memberShips = await _memberShipService.GetListByIds(task.ProjectId, setAssigneesDto.MemberShipIds);
        task.Assignees = memberShips.Select(x => new Assignee() { MemberShipId = x.Id }).ToList();

        await _repository.Update(task);
    }

    public async Task SetAssigneeTeams(Guid id, SetAssigneeTeamsDto setAssigneeTeamsDto)
    {
        var task = await FindTask(id);
        var teams = await _teamService.GetListByIds(task.ProjectId, setAssigneeTeamsDto.TeamIds);
        task.AssigneeTeams = teams.Select(x => new AssigneeTeam() { TeamId = x.Id }).ToList();

        await _repository.Update(task);
    }

    private async Task<TaskEntity> FindTask(Guid taskId)
    {
        var task = await _repository.FindById(taskId);
        if (task == default)
            throw new NotFoundException("Task not found");

        return task;
    }
}