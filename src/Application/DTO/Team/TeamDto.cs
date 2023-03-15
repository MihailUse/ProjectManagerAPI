namespace Application.DTO.Team;

public class TeamDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Color { get; set; } = null!;
}