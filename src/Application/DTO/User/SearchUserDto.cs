using Application.DTO.Common;

namespace Application.DTO.User;

public record SearchUserDto : PaginatedListQuery
{
    public string? Search { get; set; }
}