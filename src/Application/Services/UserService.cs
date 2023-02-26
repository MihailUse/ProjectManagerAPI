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
    private readonly IDatabaseContext _context;
    private readonly IIdentityService _identityService;
    private readonly IDatabaseFunctions _databaseFunctions;
    private readonly ICurrentUserService _currentUserService;

    public UserService(
        IMapper mapper,
        IDatabaseContext context,
        IIdentityService identityService,
        IDatabaseFunctions databaseFunctions,
        ICurrentUserService currentUserService
    )
    {
        _mapper = mapper;
        _context = context;
        _identityService = identityService;
        _databaseFunctions = databaseFunctions;
        _currentUserService = currentUserService;
    }

    public async Task<UserDto> GetUser(Guid userId)
    {
        var user = await FindUser(userId);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<PaginatedList<UserBriefDto>> GetUsers(GetUsersDto getUsersDto)
    {
        var query = string.IsNullOrEmpty(getUsersDto.Search)
            ? _context.Users
            : _context.Users.Where(x => _databaseFunctions.ILike(x.Login, $"%{getUsersDto.Search}%"));

        return await query.ProjectToPaginatedListAsync<UserBriefDto>(_mapper.ConfigurationProvider, getUsersDto);
    }

    public async Task<AccessTokensDto> CreateUser(CreateUserDto createUserDto)
    {
        return await _identityService.CreateUserAsync(createUserDto.Login, createUserDto.Password);
    }

    public async Task UpdateUser(Guid userId, UpdateUserDto updateUserDto)
    {
        var user = await FindUser(userId);
        user = _mapper.Map(updateUserDto, user);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(Guid userId)
    {
        if (_currentUserService.UserId != userId)
            throw new AccessDeniedException("No permission");

        var user = await FindUser(userId);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    private async Task<User> FindUser(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == default)
            throw new NotFoundException("User not found");

        return user;
    }
}