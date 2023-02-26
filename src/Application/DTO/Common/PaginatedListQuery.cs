namespace Application.DTO.Common;

public record PaginatedListQuery
{
    public virtual int PageNumber { get; init; } = 1;
    public virtual int PageSize { get; init; } = 20;
}