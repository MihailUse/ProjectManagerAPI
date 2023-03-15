namespace Domain.Entities;

public class Project : Timestamp
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid LogoId { get; set; }

    public Attach Logo { get; set; } = null!;
    public List<Task> Tasks { get; set; } = null!;
    public List<Status> Statuses { get; set; } = null!;
    public List<MemberShip> Memberships { get; set; } = null!;
    public List<ProjectAttach> ProjectAttaches { get; set; } = null!;
}