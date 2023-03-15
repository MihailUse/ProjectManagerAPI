namespace Application.DTO.Team;

public class TeamBriefDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
}