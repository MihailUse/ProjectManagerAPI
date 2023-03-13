using Application.DTO.Common;
using Application.DTO.Team;
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

public class TeamService : ITeamService
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _database;
    private readonly Guid _currentUserId;

    public TeamService(
        IMapper mapper,
        IDatabaseContext database,
        ICurrentUserService currentUserService
    )
    {
        _mapper = mapper;
        _database = database;
        _currentUserId = currentUserService.UserId;
    }

    public async Task<PaginatedList<TeamBriefDto>> GetList(Guid projectId, SearchTeamDto searchDto)
    {
        var query = _database.Teams.Where(x => x.ProjectId == projectId);

        if (string.IsNullOrEmpty(searchDto.Search))
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchDto.Search}%"));

        return await query.ProjectToPaginatedListAsync<TeamBriefDto>(_mapper.ConfigurationProvider, searchDto);
    }

    public async Task<TeamDto> GetById(Guid projectId, Guid id)
    {
        var team = await FindTeam(projectId, id);
        return _mapper.Map<TeamDto>(team);
    }

    public async Task<Guid> Create(Guid projectId, CreateTeamDto createDto)
    {
        await CheckPermission(projectId, Role.Administrator);

        var team = _mapper.Map<Team>(createDto);
        team.ProjectId = projectId;

        await _database.Teams.AddAsync(team);
        await _database.SaveChangesAsync();

        return team.Id;
    }

    public async Task Update(Guid projectId, Guid id, UpdateTeamDto updateDto)
    {
        await CheckPermission(projectId, Role.Administrator);

        var team = await FindTeam(projectId, id);
        team = _mapper.Map(updateDto, team);

        _database.Teams.Update(team);
        await _database.SaveChangesAsync();
    }

    public async Task Delete(Guid projectId, Guid id)
    {
        await CheckPermission(projectId, Role.Administrator);

        var team = await FindTeam(projectId, id);
        _database.Teams.Remove(team);
        await _database.SaveChangesAsync();
    }

    private async Task<Team> FindTeam(Guid projectId, Guid id)
    {
        var team = await _database.Teams.FirstOrDefaultAsync(x => x.Id == id && x.ProjectId == projectId);
        if (team == default)
            throw new NotFoundException("Team not found");

        return team;
    }

    private async Task CheckPermission(Guid projectId, Role role)
    {
        var isOwner = await _database.MemberShips.AnyAsync(x =>
            x.UserId == _currentUserId &&
            x.ProjectId == projectId &&
            x.Role <= role);

        if (!isOwner)
            throw new AccessDeniedException("No permission");
    }
}