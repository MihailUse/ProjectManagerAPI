using Application.DTO.Common;

namespace Application.DTO.Project;

public record SearchProjectDto : PaginatedListQuery
{
    public string? Search { get; set; }
}