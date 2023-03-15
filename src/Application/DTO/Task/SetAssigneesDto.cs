namespace Application.DTO.Task;

public class SetAssigneesDto
{
    public List<Guid>? AssigneeIds { get; set; }
    public Guid? AssigneeTeamId { get; set; }
}