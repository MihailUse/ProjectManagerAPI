using DAL.Interfaces;

namespace DAL.Entities;

public class Comment : ITimestamp, ISoftDeletable
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;

    public Task Task { get; set; } = null!;
    public User Owner { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}