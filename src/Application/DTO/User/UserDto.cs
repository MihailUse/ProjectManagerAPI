using Application.DTO.Task;
using Domain.Entities;

namespace Application.DTO.User;

public class UserDto : Timestamp
{
    public Guid Id { get; set; }
    public Guid AvatarId { get; set; }
    public string Login { get; set; } = null!;
    public string? About { get; set; }
    public int CountCommonProjects { get; set; }
    public int CountAssignedTasks { get; set; }
    public int CountCreatedProjects { get; set; }
    public int CountCreatedTasks { get; set; }
    public int CountWrittenComments { get; set; }
    public List<TaskBriefDto> AssignedTasks { get; set; } = null!;
}