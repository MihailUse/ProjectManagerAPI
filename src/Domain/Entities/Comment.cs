namespace Domain.Entities;

public class Comment : Timestamp
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;

    public Guid TaskId { get; set; }
    public Guid OwnerId { get; set; }

    public Task Task { get; set; } = null!;
    public User Owner { get; set; } = null!;
}