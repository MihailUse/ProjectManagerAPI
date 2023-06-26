using Application.DTO.Common;
using Application.DTO.User;
using Infrastructure.Extensions;
using Application.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMapper _mapper;
    private readonly DatabaseContext _database;

    public UserRepository(IMapper mapper, DatabaseContext database)
    {
        _mapper = mapper;
        _database = database;
    }

    public async Task<User?> FindById(Guid id)
    {
        return await _database.Users.FindAsync(id);
    }

    public async Task<UserDto?> FindByIdProjection(Guid id, Guid currentUserId)
    {
        return await _database.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider, new { currentUserId })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PaginatedList<UserBriefDto>> GetList(SearchUserDto searchDto)
    {
        IQueryable<User> query = _database.Users;

        if (!string.IsNullOrEmpty(searchDto.Search))
            query = query.Where(x => EF.Functions.ILike(x.Login, $"%{searchDto.Search}%"));

        return await query.ProjectToPaginatedList<User, UserBriefDto>(_mapper.ConfigurationProvider, searchDto);
    }

    public async Task Update(User user)
    {
        _database.Users.Update(user);
        await _database.SaveChangesAsync();
    }

    public async Task Add(User user)
    {
        await _database.Users.AddAsync(user);
        await _database.SaveChangesAsync();
    }

    public async Task Remove(User user)
    {
        _database.Users.Remove(user);
        await _database.SaveChangesAsync();
    }

    public async Task<bool> CheckExists(Guid userId)
    {
        return await _database.Users.AnyAsync(x => x.Id == userId);
    }

    public async Task<bool> CheckLoginExists(string login)
    {
        return await _database.Users.AnyAsync(x => x.Login == login);
    }
}