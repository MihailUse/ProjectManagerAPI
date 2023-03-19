using Application.DTO.Common;
using Application.DTO.Team;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly IMapper _mapper;
    private readonly DatabaseContext _database;

    public TeamRepository(IMapper mapper, DatabaseContext database)
    {
        _mapper = mapper;
        _database = database;
    }

    public async Task<Team?> FindById(Guid id)
    {
        return await _database.Teams.FindAsync(id);
    }

    public async Task<List<Team>> GetListByIds(Guid projectId, List<Guid> teamIds)
    {
        return await _database.Teams
            .Where(x => x.ProjectId == projectId && teamIds.Contains(x.Id))
            .ToListAsync();
    }

    public async Task<PaginatedList<TeamDto>> GetList(Guid projectId, SearchTeamDto searchDto)
    {
        var query = _database.Teams.Where(x => x.ProjectId == projectId);

        if (string.IsNullOrEmpty(searchDto.Search))
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchDto.Search}%"));

        return await query.ProjectToPaginatedList<Team, TeamDto>(_mapper.ConfigurationProvider, searchDto);
    }

    public async Task Add(Team team)
    {
        await _database.Teams.AddAsync(team);
        await _database.SaveChangesAsync();
    }

    public async Task Update(Team team)
    {
        _database.Teams.Update(team);
        await _database.SaveChangesAsync();
    }

    public async Task Remove(Team team)
    {
        _database.Teams.Remove(team);
        await _database.SaveChangesAsync();
    }
}