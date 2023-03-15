using Domain.Entities;

namespace Application.DTO.User;

public class UserDto : Timestamp
{
    public Guid Id { get; set; }
    public Guid AvatarId { get; set; }
    public string Login { get; set; } = null!;
    public string? About { get; set; }
}