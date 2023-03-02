namespace Domain.Entities;

public class Assignee
{
    public Guid TaskId { get; set; }
    public Guid MemberShipId { get; set; }

    public Task Task { get; set; } = null!;
    public MemberShip MemberShip { get; set; } = null!;
} 