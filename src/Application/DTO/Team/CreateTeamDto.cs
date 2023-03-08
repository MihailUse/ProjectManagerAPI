namespace Application.DTO.Team;

public class CreateTeamDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Color { get; set; } = null!;
}