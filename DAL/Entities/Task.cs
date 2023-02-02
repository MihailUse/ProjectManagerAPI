using DAL.Interfaces;

namespace DAL.Entities;

public class Task : ITimestamp, ISoftDeletable
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public User Owner { get; set; } = null!;
    public Status Status { get; set; } = null!;
    public Project Project { get; set; } = null!;
    public List<Comment> Comments { get; set; } = null!;
    public List<Assignee> Assignees { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}