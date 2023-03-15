using Application.DTO.Common;
using Application.DTO.Project;
using Application.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IMapper _mapper;
    private readonly DatabaseContext _database;

    public ProjectRepository(IMapper mapper, DatabaseContext database)
    {
        _mapper = mapper;
        _database = database;
    }

    public async Task<Project?> FindById(Guid id)
    {
        return await _database.Projects.FindAsync(id);
    }

    public async Task<ProjectDto?> FindByIdProjection(Guid id)
    {
        return await _database.Projects
            .Where(x => x.Id == id)
            .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task<Project?> FindByIdWithMembership(Guid id, Guid memberShipId)
    {
        return await _database.Projects
            .Include(x => x.Memberships.Where(m => m.UserId == memberShipId))
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PaginatedList<ProjectBriefDto>> GetListByMemberShip(SearchProjectDto searchDto, Guid memberShipId)
    {
        var query = _database.Projects
            .Where(x => x.Memberships.Any(m => m.UserId == memberShipId));

        if (!string.IsNullOrEmpty(searchDto.Search))
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchDto.Search}%"));

        return await query.ProjectToPaginatedList<Project, ProjectBriefDto>(_mapper.ConfigurationProvider,
            searchDto);
    }

    public async Task Add(Project project)
    {
        await _database.Projects.AddAsync(project);
        await _database.SaveChangesAsync();
    }

    public async Task Update(Project project)
    {
        _database.Projects.Update(project);
        await _database.SaveChangesAsync();
    }

    public async Task Remove(Project project)
    {
        _database.Projects.Remove(project);
        await _database.SaveChangesAsync();
    }

    public async Task<bool> CheckExists(Guid projectId)
    {
        return await _database.Projects.AnyAsync(x => x.Id == projectId);
    }
}