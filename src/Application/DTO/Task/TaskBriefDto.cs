using Application.DTO.Team;
using Application.DTO.User;

namespace Application.DTO.Task;

public class TaskBriefDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public TeamDto? AssigneeTeam { get; set; }
    public string Status { get; set; } = null!;
    public List<UserBriefDto> Assignees { get; set; } = null!;
}