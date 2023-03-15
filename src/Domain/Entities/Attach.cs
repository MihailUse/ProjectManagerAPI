namespace Domain.Entities;

public class Attach
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public long Size { get; set; } // in bytes

    public List<User> Users { get; set; } = null!;
    public List<ProjectAttach> ProjectAttaches { get; set; } = null!;
}