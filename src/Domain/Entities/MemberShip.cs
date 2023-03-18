using Domain.Enums;

namespace Domain.Entities;

public class MemberShip
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public Role Role { get; set; }

    public User User { get; set; } = null!;
    public Project Project { get; set; } = null!;
    public List<Team> Teams { get; set; } = null!;
    public List<Assignee> Assignees { get; set; } = null!;

    public MemberShip(Guid userId, Role role)
    {
        UserId = userId;
        Role = role;
    }
}