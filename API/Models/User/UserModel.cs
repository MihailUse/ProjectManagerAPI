namespace API.Models.User;

public class UserModel
{
    public Guid Id { get; set; }
    public string Login { get; set; } = null!;
    public string? About { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}