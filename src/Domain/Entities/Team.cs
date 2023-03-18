namespace Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Color { get; set; } = null!;

    public Guid ProjectId { get; set; }

    public Project Project { get; set; } = null!;
    public List<MemberShip> MemberShips { get; set; } = null!;
    public List<AssigneeTeam> AssigneeTeams { get; set; } = null!;
}