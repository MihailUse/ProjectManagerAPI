using Application.DTO.Common;
using Application.DTO.Status;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly IMapper _mapper;
    private readonly DatabaseContext _database;

    public StatusRepository(IMapper mapper, DatabaseContext database)
    {
        _mapper = mapper;
        _database = database;
    }

    public async Task<Status?> FindById(Guid id)
    {
        return await _database.Statuses.FindAsync(id);
    }

    public async Task<Status?> FindByName(string name, Guid? projectId)
    {
        return await _database.Statuses.FirstOrDefaultAsync(x =>
            x.Name == name &&
            (x.ProjectId == null || x.ProjectId == projectId)
        );
    }

    public async Task<PaginatedList<StatusDto>> GetList(Guid projectId, SearchStatusDto searchDto)
    {
        var query = _database.Statuses
            .Where(x => x.ProjectId == null || x.ProjectId == projectId);

        if (!string.IsNullOrEmpty(searchDto.Search))
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchDto.Search}%"));

        return await query.ProjectToPaginatedList<Status, StatusDto>(_mapper.ConfigurationProvider, searchDto);
    }

    public async Task Add(Status status)
    {
        await _database.Statuses.AddAsync(status);
        await _database.SaveChangesAsync();
    }

    public async Task Update(Status status)
    {
        _database.Statuses.Update(status);
        await _database.SaveChangesAsync();
    }

    public async Task Remove(Status status)
    {
        _database.Statuses.Remove(status);
        await _database.SaveChangesAsync();
    }

    public async Task<bool> CheckExists(Guid id)
    {
        return await _database.Statuses.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> CheckExists(string name, Guid? projectId)
    {
        return await _database.Statuses.AnyAsync(x =>
            x.Name == name &&
            (x.ProjectId == null || x.ProjectId == projectId)
        );
    }
}