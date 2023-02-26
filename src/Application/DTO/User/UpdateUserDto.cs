namespace Application.DTO.User;

public class UpdateUserDto
{
    public string Login { get; set; } = null!;
    public string? About { get; set; }
}