using Application.DTO.MemberShip;
using Application.DTO.Team;

namespace Application.DTO.Task;

public class TaskBriefDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Status { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public TeamDto? AssigneeTeam { get; set; }
    public List<MemberShipDto> Assignees { get; set; } = null!;
}