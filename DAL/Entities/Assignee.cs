namespace DAL.Entities;

public class Assignee
{
    public Guid UserId { get; set; }
    public Guid TaskId { get; set; }

    public User User { get; set; } = null!;
    public Task Task { get; set; } = null!;
}