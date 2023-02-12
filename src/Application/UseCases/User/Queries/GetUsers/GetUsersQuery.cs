using Application.Common.DTO.User;
using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Queries.GetUsers;

public record GetUsersQuery : IRequest<PaginatedList<UserBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
