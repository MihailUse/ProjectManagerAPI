using Application.DTO.Common;
using Application.DTO.MemberShip;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Repositories;

public class MemberShipRepository : IMemberShipRepository
{
    private readonly IMapper _mapper;
    private readonly DatabaseContext _database;

    public MemberShipRepository(IMapper mapper, DatabaseContext database)
    {
        _mapper = mapper;
        _database = database;
    }

    public async Task<MemberShip?> FindById(Guid id)
    {
        return await _database.MemberShips.FindAsync(id);
    }

    public async Task<List<MemberShip>> GetByIds(Guid projectId, List<Guid> memberShipIds)
    {
        return await _database.MemberShips
            .Where(x => x.ProjectId == projectId && memberShipIds.Contains(x.Id))
            .ToListAsync();
    }

    public async Task<PaginatedList<MemberShipDto>> GetList(Guid projectId, SearchMemberShipDto searchDto)
    {
        var query = _database.MemberShips
            .Where(x => x.ProjectId == projectId);

        if (searchDto.Role != default)
            query = query.Where(x => x.Role == searchDto.Role);

        if (!string.IsNullOrEmpty(searchDto.SearchByUserName))
            query = query.Where(x => EF.Functions.ILike(x.User.Login, $"%{searchDto.SearchByUserName}%"));

        return await query.ProjectToPaginatedList<MemberShip, MemberShipDto>(_mapper.ConfigurationProvider,
            searchDto);
    }

    public async Task Add(MemberShip memberShip)
    {
        await _database.MemberShips.AddAsync(memberShip);
        await _database.SaveChangesAsync();
    }

    public async Task Update(MemberShip memberShip)
    {
        _database.MemberShips.Update(memberShip);
        await _database.SaveChangesAsync();
    }

    public async Task Remove(MemberShip memberShip)
    {
        _database.MemberShips.Remove(memberShip);
        await _database.SaveChangesAsync();
    }

    public async Task<bool> CheckExists(Guid projectId, Guid userId)
    {
        return await _database.MemberShips.AnyAsync(x => x.ProjectId == projectId && x.UserId == userId);
    }
}