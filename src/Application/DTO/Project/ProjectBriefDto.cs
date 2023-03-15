namespace Application.DTO.Project;

public class ProjectBriefDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid LogoId { get; set; }
}