using Application.DTO.Common;
using Application.DTO.Task;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;
using TaskEntity = Domain.Entities.Task;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _database;
    private readonly IDatabaseFunctions _databaseFunctions;
    private readonly Guid _currentUserId;

    public TaskService(
        IMapper mapper,
        IDatabaseContext database,
        IDatabaseFunctions databaseFunctions,
        ICurrentUserService currentUserService
    )
    {
        _mapper = mapper;
        _database = database;
        _databaseFunctions = databaseFunctions;
        _currentUserId = currentUserService.UserId;
    }

    public async Task<TaskDto> GetById(Guid id)
    {
        var task = await _database.Tasks
            .Where(x => x.Id == id && x.Project.Memberships.Any(m => m.UserId == _currentUserId))
            .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        if (task == default)
            throw new NotFoundException("Task not found");

        return task;
    }

    public async Task<PaginatedList<TaskBriefDto>> GetList(SearchTaskDto searchDto)
    {
        var query = _database.Tasks
            .Where(x => x.Project.Memberships.Any(m => m.UserId == _currentUserId));

        if (!string.IsNullOrEmpty(searchDto.Search))
            query = query.Where(x => _databaseFunctions.ILike(x.Title, $"%{searchDto.Search}%"));

        if (searchDto.StatusId != default)
            query = query.Where(x => x.StatusId == searchDto.StatusId);

        if (searchDto.ProjectId != default)
            query = query.Where(x => x.ProjectId == searchDto.ProjectId);

        return await query.ProjectToPaginatedListAsync<TaskBriefDto>(_mapper.ConfigurationProvider, searchDto);
    }

    public async Task<Guid> Create(CreateTaskDto createDto)
    {
        // check project
        var projectExists = await _database.MemberShips.AnyAsync(x =>
            x.UserId == _currentUserId &&
            x.ProjectId == createDto.ProjectId &&
            x.Role <= Role.MemberShip);
        if (!projectExists)
            throw new NotFoundException("Project not found");

        // check status
        await CheckStatusExists(createDto.StatusId);

        var task = _mapper.Map<TaskEntity>(createDto);
        task.OwnerId = _currentUserId;

        await _database.Tasks.AddAsync(task);
        await _database.SaveChangesAsync();

        return task.Id;
    }

    public async Task Update(Guid id, UpdateTaskDto updateDto)
    {
        var task = await FindTask(id);
        if (task.OwnerId != _currentUserId)
            await CheckPermission(id, Role.Administrator);

        if (task.StatusId != updateDto.StatusId)
            await CheckStatusExists(updateDto.StatusId);

        task = _mapper.Map(updateDto, task);
        _database.Tasks.Update(task);
        await _database.SaveChangesAsync();
    }


    public async Task Delete(Guid id)
    {
        var task = await FindTask(id);
        await CheckPermission(id, Role.Administrator);
        _database.Tasks.Remove(task);
        await _database.SaveChangesAsync();
    }

    public async Task SetAssignees(Guid id, SetAssigneesDto setAssigneesDto)
    {
        var task = await _database.Tasks
            .Include(x => x.Assignees)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.Project.Memberships.Any(m => m.UserId == _currentUserId));
        if (task == default)
            throw new NotFoundException("Task not found");

        if (setAssigneesDto.AssigneeTeamId != default)
        {
            var team = await _database.Teams.FirstOrDefaultAsync(x =>
                x.Id == setAssigneesDto.AssigneeTeamId && x.ProjectId == task.ProjectId);
            if (team == default)
                throw new NotFoundException("Team not found");

            task.AssigneeTeamId = team.Id;
        }

        if (setAssigneesDto.AssigneeIds != default)
        {
            var assignedMemberShipIds = await _database.MemberShips
                .Where(x => x.ProjectId == task.ProjectId && setAssigneesDto.AssigneeIds.Contains(x.UserId))
                .Select(x => x.Id)
                .ToListAsync();
            if (assignedMemberShipIds.Count != setAssigneesDto.AssigneeIds.Count)
                throw new NotFoundException("Some memberships not found");

            task.Assignees = assignedMemberShipIds
                .Select(x => new Assignee() { MemberShipId = x })
                .ToList();
        }

        _database.Tasks.Update(task);
        await _database.SaveChangesAsync();
    }

    private async Task<TaskEntity> FindTask(Guid taskId)
    {
        var task = await _database.Tasks.FirstOrDefaultAsync(x =>
            x.Id == taskId &&
            x.Project.Memberships.Any(m => m.UserId == _currentUserId));
        if (task == default)
            throw new NotFoundException("Task not found");

        return task;
    }

    private async Task CheckStatusExists(Guid statusId)
    {
        var statusExists = await _database.Statuses.AnyAsync(x => x.Id == statusId);
        if (!statusExists)
            throw new NotFoundException("Task not found");
    }

    private async Task CheckPermission(Guid taskId, Role role)
    {
        var task = await _database.Tasks
            .Include(x => x.Project.Memberships.Where(m => m.UserId == _currentUserId))
            .FirstOrDefaultAsync(x => x.Id == taskId);
        if (task == default)
            throw new NotFoundException("Task not found");

        var currentMemberShip = task.Project.Memberships.FirstOrDefault();
        if (currentMemberShip == default || currentMemberShip.Role > role)
            throw new AccessDeniedException("No permission");
    }
}