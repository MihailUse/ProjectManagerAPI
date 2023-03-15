using Application.DTO.Common;

namespace Application.DTO.Comment;

public record SearchCommentDto : PaginatedListQuery
{
    public Guid TaskId { get; set; }
}