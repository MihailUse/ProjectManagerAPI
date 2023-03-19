namespace Domain.Entities;

public class AssigneeTeam
{
    public Guid TaskId { get; set; }
    public Guid TeamId { get; set; }

    public Task Task { get; set; } = null!;
    public Team Team { get; set; } = null!;
}