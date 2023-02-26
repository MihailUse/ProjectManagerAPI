using Application.DTO.Common;

namespace Application.DTO.Project;

public record GetProjectsDto : PaginatedListQuery
{
    public string? Search { get; set; }
}