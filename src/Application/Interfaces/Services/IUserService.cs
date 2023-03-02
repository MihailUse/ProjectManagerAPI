using Application.DTO.Auth;
using Application.DTO.Common;
using Application.DTO.User;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<UserDto> GetById(Guid id);
    Task<PaginatedList<UserBriefDto>> GetList(SearchUserDto searchDto);
    Task<AccessTokensDto> Create(CreateUserDto createDto);
    Task Update(Guid id, UpdateUserDto updateDto);
    Task Delete(Guid id);
}