namespace Domain.Entities;

public class Task : Timestamp
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public Guid OwnerId { get; set; }
    public Guid StatusId { get; set; }
    public Guid ProjectId { get; set; }

    public User Owner { get; set; } = null!;
    public Status Status { get; set; } = null!;
    public Project Project { get; set; } = null!;

    public List<Comment> Comments { get; set; } = null!;
    public List<Assignee> Assignees { get; set; } = null!;
    public List<AssigneeTeam> AssigneeTeams { get; set; } = null!;
}