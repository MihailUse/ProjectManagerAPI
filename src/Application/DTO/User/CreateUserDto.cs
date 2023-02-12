namespace Application.DTO.User;

public record CreateUserDto
{
    public string Login { get; init; } = null!;
    public string Password { get; init; } = null!;
}
