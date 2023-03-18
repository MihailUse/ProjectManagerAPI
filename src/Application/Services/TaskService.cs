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

    public async Task<PaginatedList<TaskBriefDto>> GetList(SearchTaskDto searchDto)
    {
        return await _repository.GetList(searchDto);
    }

    public async Task<Guid> Create(Guid projectId, CreateTaskDto createDto)
    {
        await _statusService.CheckStatusExists(createDto.StatusId);

        var task = _mapper.Map<TaskEntity>(createDto);
        task.OwnerId = _currentUserId;

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

        if (task.AssigneeTeamId != setAssigneesDto.AssigneeTeamId && setAssigneesDto.AssigneeTeamId != default)
        {
            await _teamService.CheckTeamExists(setAssigneesDto.AssigneeTeamId.GetValueOrDefault());
            task.AssigneeTeamId = setAssigneesDto.AssigneeTeamId;
        }

        if (setAssigneesDto.AssigneeIds != default)
        {
            var assignedMemberShipIds =
                await _memberShipService.GetAssignedMemberShipIds(task.ProjectId, setAssigneesDto.AssigneeIds);

            task.Assignees = assignedMemberShipIds
                .Select(x => new Assignee() { MemberShipId = x })
                .ToList();
        }

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