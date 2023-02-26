using Domain.Entities;

namespace Application.DTO.Project;

public class ProjectDto : Timestamp
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public byte[]? Logo { get; set; }
    public string? Description { get; set; }

    public int TaskCount { get; set; }
    public int MembershipCount { get; set; }
}