using Application.DTO.Common;
using Application.DTO.MemberShip;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class MemberShipService : IMemberShipService
{
    private readonly IMapper _mapper;
    private readonly IMemberShipRepository _repository;
    private readonly IProjectService _projectService;
    private readonly IUserService _userService;

    public MemberShipService(
        IMapper mapper,
        IMemberShipRepository repository,
        IProjectService projectService,
        IUserService userService
    )
    {
        _mapper = mapper;
        _repository = repository;
        _projectService = projectService;
        _userService = userService;
    }


    private async Task<MemberShip> GetById(Guid id)
    {
        var memberShip = await _repository.FindById(id);
        if (memberShip == default)
            throw new NotFoundException("MemberShip not found");

        return memberShip;
    }

    public async Task<List<Guid>> GetAssignedMemberShipIds(Guid projectId, List<Guid> userIds)
    {
        var assignedMemberShipIds = await _repository.GetIdsByUserIds(projectId, userIds);
        if (assignedMemberShipIds.Count != userIds.Count)
            throw new NotFoundException("Some memberships not found");

        return assignedMemberShipIds;
    }

    public async Task<PaginatedList<MemberShipDto>> GetList(Guid projectId, SearchMemberShipDto searchDto)
    {
        return await _repository.GetList(projectId, searchDto);
    }

    public async Task<List<MemberShip>> GetListByIds(List<Guid> memberShipIds)
    {
        var memberShips = await _repository.GetByIds(memberShipIds);
        if (memberShips.Count != memberShipIds.Count)
            throw new NotFoundException("Some memberships not found");

        return memberShips;
    }

    public async Task<Guid> Create(Guid projectId, CreateMemberShipDto createDto)
    {
        var memberShipExists = await _repository.CheckExists(projectId, createDto.UserId);
        if (memberShipExists)
            throw new ConflictException("MemberShip already exists in project");

        await _projectService.CheckProjectExists(projectId);
        await _userService.CheckUserExists(createDto.UserId);

        var memberShip = _mapper.Map<MemberShip>(createDto);
        await _repository.Add(memberShip);
        return memberShip.Id;
    }

    public async Task Update(Guid id, UpdateMemberShipDto updateDto)
    {
        var memberShip = await GetById(id);
        if (memberShip.Role == Role.Owner)
            throw new InvalidOperationException("Can not update project owner");

        memberShip = _mapper.Map(updateDto, memberShip);
        await _repository.Update(memberShip);
    }

    public async Task Delete(Guid id)
    {
        var memberShip = await GetById(id);
        if (memberShip.Role == Role.Owner)
            throw new InvalidOperationException("Can not delete project owner");

        await _repository.Remove(memberShip);
    }
}