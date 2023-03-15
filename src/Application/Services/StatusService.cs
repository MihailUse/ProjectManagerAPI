using Application.DTO.Common;
using Application.DTO.Status;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class StatusService : IStatusService
{
    private readonly IMapper _mapper;
    private readonly IStatusRepository _repository;

    public StatusService(IMapper mapper, IStatusRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<StatusDto>> GetList(Guid projectId, SearchStatusDto searchDto)
    {
        return await _repository.GetList(projectId, searchDto);
    }

    public async Task<Guid> Create(Guid projectId, CreateStatusDto createDto)
    {
        var nameExists = await _repository.CheckExists(createDto.Name, projectId);
        if (nameExists)
            throw new ConflictException("Status already exists in project");

        var status = _mapper.Map<Status>(createDto);
        await _repository.Add(status);
        return status.Id;
    }

    public async Task Update(Guid id, UpdateStatusDto updateDto)
    {
        var status = await FindStatus(id);
        if (status.ProjectId == default)
            throw new InvalidOperationException("Can't update default status");

        var existsStatus = await _repository.FindByName(updateDto.Name, status.ProjectId);
        if (existsStatus != default && existsStatus.Id != id)
            throw new ConflictException("Status already exists in project");

        status = _mapper.Map(updateDto, status);
        await _repository.Update(status);
    }

    public async Task Delete(Guid id)
    {
        var status = await FindStatus(id);
        if (status.ProjectId == default)
            throw new InvalidOperationException("Can't delete default status");

        await _repository.Remove(status);
    }

    public async Task CheckStatusExists(Guid statusId)
    {
        var statusExists = await _repository.CheckExists(statusId);
        if (!statusExists)
            throw new NotFoundException("Task not found");
    }

    private async Task<Status> FindStatus(Guid id)
    {
        var status = await _repository.FindById(id);
        if (status == default)
            throw new NotFoundException("Status not found");

        return status;
    }
}