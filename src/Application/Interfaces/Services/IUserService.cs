using Application.DTO.Auth;
using Application.DTO.Common;
using Application.DTO.User;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<UserDto> GetUser(Guid userId);
    Task<PaginatedList<UserBriefDto>> GetUsers(GetUsersDto getUsersDto);
    Task<AccessTokensDto> CreateUser(CreateUserDto createUserDto);
    Task UpdateUser(Guid userId, UpdateUserDto updateUserDto);
    Task DeleteUser(Guid userId);
}