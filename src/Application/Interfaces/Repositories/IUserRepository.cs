using Application.DTO.Common;
using Application.DTO.User;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> FindById(Guid id);
    Task<UserDto?> FindByIdProjection(Guid id, Guid currentUserId);
    Task<PaginatedList<UserBriefDto>> GetList(SearchUserDto searchDto);
    Task Update(User user);
    Task Add(User user);
    Task Remove(User user);
    Task<bool> CheckExists(Guid userId);
    Task<bool> CheckLoginExists(string login);
}