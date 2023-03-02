using Application.DTO.Auth;
using Application.DTO.Common;
using Application.DTO.User;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

internal class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _database;
    private readonly IIdentityService _identityService;
    private readonly IDatabaseFunctions _databaseFunctions;
    private readonly ICurrentUserService _currentUserService;

    public UserService(
        IMapper mapper,
        IDatabaseContext database,
        IIdentityService identityService,
        IDatabaseFunctions databaseFunctions,
        ICurrentUserService currentUserService
    )
    {
        _mapper = mapper;
        _database = database;
        _identityService = identityService;
        _databaseFunctions = databaseFunctions;
        _currentUserService = currentUserService;
    }

    public async Task<UserDto> GetById(Guid id)
    {
        var user = await FindUser(id);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<PaginatedList<UserBriefDto>> GetList(SearchUserDto searchDto)
    {
        IQueryable<User> query = _database.Users;

        if (!string.IsNullOrEmpty(searchDto.Search))
            query = query.Where(x => _databaseFunctions.ILike(x.Login, $"%{searchDto.Search}%"));

        return await query.ProjectToPaginatedListAsync<UserBriefDto>(_mapper.ConfigurationProvider, searchDto);
    }

    public async Task<AccessTokensDto> Create(CreateUserDto createDto)
    {
        return await _identityService.CreateUserAsync(createDto.Login, createDto.Password);
    }

    public async Task Update(Guid id, UpdateUserDto updateDto)
    {
        var user = await FindUser(id);
        user = _mapper.Map(updateDto, user);

        _database.Users.Update(user);
        await _database.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        if (_currentUserService.UserId != id)
            throw new AccessDeniedException("No permission");

        var user = await FindUser(id);
        _database.Users.Remove(user);
        await _database.SaveChangesAsync();
    }

    private async Task<User> FindUser(Guid userId)
    {
        var user = await _database.Users.FindAsync(userId);
        if (user == default)
            throw new NotFoundException("User not found");

        return user;
    }
}