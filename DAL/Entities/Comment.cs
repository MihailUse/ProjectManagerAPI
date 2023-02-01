using DAL.Interfaces;

namespace DAL.Entities;

public class Comment : ITimestamp, ISoftDeletable
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;

    public Task Task { get; set; } = null!;
    public User Owner { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}