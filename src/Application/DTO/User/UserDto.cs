using Domain.Entities;

namespace Application.DTO.User;

public class UserDto : Timestamp
{
    public Guid Id { get; set; }
    public string Login { get; set; } = null!;
    public string? About { get; set; }
    public byte[] Avatar { get; set; } = null!;
}