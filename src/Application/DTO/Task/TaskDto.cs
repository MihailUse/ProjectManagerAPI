using Application.DTO.MemberShip;
using Application.DTO.Team;
using Application.DTO.User;
using Domain.Entities;

namespace Application.DTO.Task;

public class TaskDto : Timestamp
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = null!;

    public UserBriefDto Owner { get; set; } = null!;
    public TeamDto? AssigneeTeam { get; set; }
    public List<MemberShipDto> Assignees { get; set; } = null!;
}