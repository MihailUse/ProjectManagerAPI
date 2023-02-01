using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(Login), IsUnique = true)]
public class User : ITimestamp, ISoftDeletable
{
    public Guid Id { get; set; }
    public string Login { get; set; } = null!;
    public string? About { get; set; }
    public byte[] Avatar { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public List<Task> Tasks { get; set; } = null!;
    public List<Project> Projects { get; set; } = null!;
    public List<Comment> Comments { get; set; } = null!;
    public List<Assignee> Assignees { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}