using Application.DTO.Auth;
using Application.DTO.User;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Mappings;
using Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

internal class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _context;
    private readonly IIdentityService _identityService;

    public UserService(IMapper mapper, IDatabaseContext context, IIdentityService identityService)
    {
        _mapper = mapper;
        _context = context;
        _identityService = identityService;
    }

    public async Task<AccessTokensDto> CreateUser(CreateUserDto createUserDto)
    {
        return await _identityService.CreateUserAsync(createUserDto.Login, createUserDto.Password);
    }

    public async Task<PaginatedList<UserBriefDto>> GetUsers(GetUsersDto getUsersDto)
    {
        var query = string.IsNullOrEmpty(getUsersDto.Search)
            ? _context.Users
            : _context.Users.Where(x => EF.Functions.ILike(x.Login, $"%{getUsersDto.Search}%"));

        return await query.ProjectToPaginatedListAsync<UserBriefDto>(_mapper.ConfigurationProvider, getUsersDto);
    }
}