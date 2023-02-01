namespace DAL.Entities;

public class MemberShip
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }

    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public Project Project { get; set; } = null!;
    public List<Team> Teams { get; set; } = null!;
}