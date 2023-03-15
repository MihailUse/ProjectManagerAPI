using Application.DTO.Common;
using Application.DTO.Team;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
// using Domain.Enums;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class TeamService : ITeamService
{
    private readonly IMapper _mapper;
    private readonly ITeamRepository _repository;
    private readonly IProjectService _projectService;

    public TeamService(
        IMapper mapper,
        ITeamRepository repository,
        IProjectService projectService
    )
    {
        _mapper = mapper;
        _repository = repository;
        _projectService = projectService;
    }

    public async Task<PaginatedList<TeamDto>> GetList(Guid projectId, SearchTeamDto searchDto)
    {
        return await _repository.GetList(projectId, searchDto);
    }

    public async Task<Guid> Create(Guid projectId, CreateTeamDto createDto)
    {
        // await _projectService.CheckPermission(projectId, Role.Administrator);

        var team = _mapper.Map<Team>(createDto);
        team.ProjectId = projectId;

        await _repository.Add(team);
        return team.Id;
    }

    public async Task Update(Guid id, UpdateTeamDto updateDto)
    {
        // await _projectService.CheckPermission(projectId, Role.Administrator);

        var team = await FindTeam(id);
        team = _mapper.Map(updateDto, team);

        await _repository.Update(team);
    }

    public async Task Delete(Guid id)
    {
        // await _projectService.CheckPermission(projectId, Role.Administrator);

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