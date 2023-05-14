using Application.DTO.Common;

namespace Application.DTO.Comment;

public record SearchCommentDto : PaginatedListQuery
{
    public string? Search { get; set; }
}