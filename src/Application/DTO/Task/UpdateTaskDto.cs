namespace Application.DTO.Task;

public class UpdateTaskDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public Guid StatusId { get; set; }
}