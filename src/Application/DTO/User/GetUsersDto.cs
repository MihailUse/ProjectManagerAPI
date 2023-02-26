using Application.Models;

namespace Application.DTO.User;

public record GetUsersDto : PaginatedListQuery
{
    public string? Search { get; set; }
}