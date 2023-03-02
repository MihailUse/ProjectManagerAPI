namespace Application.DTO.Status;

public class CreateStatusDto
{
    public string Name { get; set; } = null!;
    public Guid ProjectId { get; set; }
}