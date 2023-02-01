using System.ComponentModel.DataAnnotations;

namespace API.Models.User;

public class UpdateUserModel
{
    [MinLength(3), MaxLength(32)]
    public string? Login { get; set; }

    [MaxLength(512)]
    public string? About { get; set; }
}