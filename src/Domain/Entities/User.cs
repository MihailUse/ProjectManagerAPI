namespace Domain.Entities;

public class User : Timestamp
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
    public List<UserSession> UserSessions { get; set; } = null!;
}