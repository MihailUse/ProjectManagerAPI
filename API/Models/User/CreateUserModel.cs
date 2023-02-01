using System.ComponentModel.DataAnnotations;

namespace API.Models.User;

public class CreateUserModel
{
    [MinLength(3), MaxLength(32)]
    public string Login { get; set; } = null!;

    [MaxLength(512)]
    public string? About { get; set; }

    [MinLength(6), MaxLength(128)]
    public string Password { get; set; } = null!;

    [MinLength(6), MaxLength(128), Compare(nameof(Password))]
    public string RetryPassword { get; set; } = null!;
}