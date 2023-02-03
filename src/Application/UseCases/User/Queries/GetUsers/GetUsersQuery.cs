using Application.Common.DTO;
using Application.Common.Models;
using MediatR;

namespace Application.UseCases.User.Queries.GetUsersWithPagination;

public record GetUsersQuery : IRequest<PaginatedList<UserBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
