using Application.DTO.Common;
using Application.DTO.Task;
using Application.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;
using TaskEntity = Domain.Entities.Task;

namespace Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IMapper _mapper;
    private readonly DatabaseContext _database;

    public TaskRepository(IMapper mapper, DatabaseContext database)
    {
        _mapper = mapper;
        _database = database;
    }

    public async Task<TaskEntity?> FindById(Guid id)
    {
        return await _database.Tasks.FindAsync(id);
    }

    public async Task<TaskDto?> FindByIdProjection(Guid id)
    {
        return await _database.Tasks
            .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TaskEntity?> FindByIdWithMemberShip(Guid taskId, Guid memberShipId)
    {
        return await _database.Tasks
            .Include(x => x.Project.Memberships.Where(m => m.UserId == memberShipId))
            .FirstOrDefaultAsync(x => x.Id == taskId);
    }

    public async Task<PaginatedList<TaskBriefDto>> GetList(Guid projectId, SearchTaskDto searchDto)
    {
        IQueryable<TaskEntity> query = _database.Tasks
            .Where(x => x.ProjectId == projectId);

        if (!string.IsNullOrEmpty(searchDto.Search))
            query = query.Where(x => EF.Functions.ILike(x.Title, $"%{searchDto.Search}%"));

        if (searchDto.StatusId != default)
            query = query.Where(x => x.StatusId == searchDto.StatusId);

        return await query.ProjectToPaginatedList<TaskEntity, TaskBriefDto>(_mapper.ConfigurationProvider, searchDto);
    }

    public async Task AddAsync(TaskEntity task)
    {
        await _database.Tasks.AddAsync(task);
        await _database.SaveChangesAsync();
    }

    public async Task Update(TaskEntity task)
    {
        _database.Tasks.Update(task);
        await _database.SaveChangesAsync();
    }

    public async Task Remove(TaskEntity task)
    {
        _database.Tasks.Remove(task);
        await _database.SaveChangesAsync();
    }
}