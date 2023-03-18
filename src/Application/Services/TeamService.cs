using Application.DTO.Common;
using Application.DTO.Team;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class TeamService : ITeamService
{
    private readonly IMapper _mapper;
    private readonly ITeamRepository _repository;
    private readonly IMemberShipService _memberShipService;

    public TeamService(
        IMapper mapper,
        ITeamRepository repository,
        IMemberShipService memberShipService
    )
    {
        _mapper = mapper;
        _repository = repository;
        _memberShipService = memberShipService;
    }

    public async Task<PaginatedList<TeamDto>> GetList(Guid projectId, SearchTeamDto searchDto)
    {
        return await _repository.GetList(projectId, searchDto);
    }

    public async Task<Guid> Create(Guid projectId, CreateTeamDto createDto)
    {
        var team = _mapper.Map<Team>(createDto);
        team.ProjectId = projectId;

        if (createDto.MemberShipIds.Count > 0)
            team.MemberShips = await _memberShipService.GetListByIds(createDto.MemberShipIds);

        await _repository.Add(team);
        return team.Id;
    }

    public async Task Update(Guid id, UpdateTeamDto updateDto)
    {
        var team = await FindTeam(id);
        team = _mapper.Map(updateDto, team);

        if (updateDto.MemberShipIds.Count > 0)
            team.MemberShips = await _memberShipService.GetListByIds(updateDto.MemberShipIds);

        await _repository.Update(team);
    }

    public async Task Delete(Guid id)
    {
        var team = await FindTeam(id);
        await _repository.Remove(team);
    }

    public async Task CheckTeamExists(Guid teamId)
    {
        var team = await _repository.CheckExists(teamId);
        if (!team)
            throw new NotFoundException("Team not found");
    }

    private async Task<Team> FindTeam(Guid id)
    {
        var team = await _repository.FindById(id);
        if (team == default)
            throw new NotFoundException("Team not found");

        return team;
    }
}