namespace Domain.Entities;

public class Status
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public Guid? ProjectId { get; set; }

    public Project? Project { get; set; }
    public List<Task> Tasks { get; set; } = null!;

    public Status(string name) => Name = name;
}