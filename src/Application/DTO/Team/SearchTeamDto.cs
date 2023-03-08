using Application.DTO.Common;

namespace Application.DTO.Team;

public record SearchTeamDto : PaginatedListQuery
{
    public string? Search { get; set; }
}