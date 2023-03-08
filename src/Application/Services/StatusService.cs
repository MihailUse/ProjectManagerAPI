using Application.DTO.Common;
using Application.DTO.Status;
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

public class StatusService : IStatusService
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _database;
    private readonly IDatabaseFunctions _databaseFunctions;
    private readonly Guid _currentUserId;

    public StatusService(
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

    public async Task<PaginatedList<StatusDto>> GetList(SearchStatusDto searchDto)
    {
        var query = _database.Statuses
            .Where(x => x.ProjectId == null || x.ProjectId == searchDto.ProjectId);

        if (!string.IsNullOrEmpty(searchDto.Search))
            query = query.Where(x => _databaseFunctions.ILike(x.Name, $"%{searchDto.Search}%"));

        return await query.ProjectToPaginatedListAsync<StatusDto>(_mapper.ConfigurationProvider, searchDto);
    }

    public async Task<Guid> Create(CreateStatusDto createDto)
    {
        await CheckPermission(createDto.ProjectId, Role.Administrator);

        var nameExists = await _database.Statuses
            .AnyAsync(x => x.ProjectId == createDto.ProjectId && x.Name == createDto.Name);
        if (nameExists)
            throw new ConflictException("Status already exists in project");

        var status = _mapper.Map<Status>(createDto);
        await _database.Statuses.AddAsync(status);
        await _database.SaveChangesAsync();

        return status.Id;
    }

    public async Task Update(Guid id, UpdateStatusDto updateDto)
    {
        var status = await FindStatus(id);
        await CheckPermission(status.ProjectId, Role.Administrator);

        var nameExists = await _database.Statuses.AnyAsync(x =>
            x.Id != id &&
            x.ProjectId == status.ProjectId &&
            x.Name == updateDto.Name
        );
        if (nameExists)
            throw new ConflictException("Status already exists in project");

        status = _mapper.Map(updateDto, status);
        _database.Statuses.Update(status);
        await _database.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var status = await FindStatus(id);
        await CheckPermission(status.ProjectId, Role.Administrator);

        _database.Statuses.Remove(status);
        await _database.SaveChangesAsync();
    }

    private async Task<Status> FindStatus(Guid id)
    {
        var status = await _database.Statuses.FindAsync(id);
        if (status == default)
            throw new NotFoundException("Status not found");

        return status;
    }

    private async Task CheckPermission(Guid? projectId, Role role)
    {
        if (projectId == default)
            throw new AccessDeniedException("No permission");

        var status = await _database.Statuses
            .Include(x => x.Project!.Memberships.Where(m => m.UserId == _currentUserId))
            .FirstOrDefaultAsync(x => x.ProjectId == projectId);
        if (status == default)
            throw new NotFoundException("Status not found");

        var currentMemberShip = status.Project?.Memberships.FirstOrDefault();
        if (currentMemberShip == default || currentMemberShip.Role > role)
            throw new AccessDeniedException("No permission");
    }
}