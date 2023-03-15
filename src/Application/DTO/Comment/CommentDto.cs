using Application.DTO.User;
using Domain.Entities;

namespace Application.DTO.Comment;

public class CommentDto : Timestamp
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;

    public UserBriefDto Owner { get; set; } = null!;
}