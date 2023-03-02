using Application.DTO.Common;

namespace Application.DTO.Status;

public record SearchStatusDto : PaginatedListQuery
{
    public string? Search { get; set; }
    public Guid ProjectId { get; set; }
}