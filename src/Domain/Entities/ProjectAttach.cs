namespace Domain.Entities;

public class ProjectAttach
{
    public Guid AttachId { get; set; }
    public Guid ProjectId { get; set; }

    public Attach Attach { get; set; } = null!;
    public Project Project { get; set; } = null!;
}