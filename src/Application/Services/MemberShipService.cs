using Application.DTO.Common;
using Application.DTO.MemberShip;
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

public class MemberShipService : IMemberShipService
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _database;
    private readonly IDatabaseFunctions _databaseFunctions;
    private readonly Guid _currentUserId;

    public MemberShipService(
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

    public async Task<PaginatedList<MemberShipDto>> GetList(SearchMemberShipDto searchDto)
    {
        var query = _database.MemberShips
            .Where(x => x.ProjectId == searchDto.ProjectId);

        if (searchDto.Role != default)
            query = query.Where(x => x.Role == searchDto.Role);

        if (!string.IsNullOrEmpty(searchDto.SearchByUserName))
            query = query.Where(x => _databaseFunctions.ILike(x.User.Login, $"%{searchDto.SearchByUserName}%"));

        return await query.ProjectToPaginatedListAsync<MemberShipDto>(_mapper.ConfigurationProvider, searchDto);
    }

    public async Task<Guid> Create(CreateMemberShipDto createDto)
    {
        await CheckPermission(createDto.ProjectId, Role.Administrator);

        var memberShipExists = await _database.MemberShips
            .AnyAsync(x => x.ProjectId == createDto.ProjectId && x.UserId == createDto.UserId);
        if (memberShipExists)
            throw new ConflictException("MemberShip already exists in project");

        var projectExists = await _database.Projects.AnyAsync(x => x.Id == createDto.ProjectId);
        if (!projectExists)
            throw new NotFoundException("Project not found");

        var userExists = await _database.Users.AnyAsync(x => x.Id == createDto.UserId);
        if (!userExists)
            throw new NotFoundException("User not found");

        var memberShip = _mapper.Map<MemberShip>(createDto);
        await _database.MemberShips.AddAsync(memberShip);
        await _database.SaveChangesAsync();

        return memberShip.Id;
    }

    public async Task Update(Guid id, UpdateMemberShipDto updateDto)
    {
        var memberShip = await FindMemberShip(id);
        await CheckPermission(memberShip.ProjectId, Role.Administrator);

        memberShip = _mapper.Map(updateDto, memberShip);
        _database.MemberShips.Update(memberShip);
        await _database.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var memberShip = await FindMemberShip(id);
        await CheckPermission(memberShip.ProjectId, Role.Administrator);
        _database.MemberShips.Remove(memberShip);
        await _database.SaveChangesAsync();
    }

    private async Task<MemberShip> FindMemberShip(Guid id)
    {
        var memberShip = await _database.MemberShips.FindAsync(id);
        if (memberShip == default)
            throw new NotFoundException("MemberShip not found");

        return memberShip;
    }

    private async Task CheckPermission(Guid projectId, Role role)
    {
        var hasPermission = await _database.MemberShips.AnyAsync(x =>
            x.ProjectId == projectId &&
            x.UserId == _currentUserId &&
            x.Role <= role);

        if (!hasPermission)
            throw new AccessDeniedException("No permission");
    }
}