using Application.DTO.Common;

namespace Application.DTO.User;

public record GetUsersDto : PaginatedListQuery
{
    public string? Search { get; set; }
}