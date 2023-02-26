using Application.DTO.Auth;
using Application.DTO.User;
using Application.Models;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<AccessTokensDto> CreateUser(CreateUserDto createUser);
    Task<PaginatedList<UserBriefDto>> GetUsers(GetUsersDto getUsersDto);
}