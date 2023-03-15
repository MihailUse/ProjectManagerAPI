namespace Application.DTO.Comment;

public class CreateCommentDto
{
    public Guid TaskId { get; set; }
    public string Text { get; set; } = null!;
}