using Application.Common.DTO;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Mappings;
using AutoMapper;
using MediatR;

namespace Application.UseCases.User.Queries.GetUsersWithPagination;

public record GetUsersWithPaginationQuery : IRequest<PaginatedList<UserBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}

public class GetUsersWithPaginationQueryHeandler : IRequestHandler<GetUsersWithPaginationQuery, PaginatedList<UserBriefDto>>
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _context;

    public GetUsersWithPaginationQueryHeandler(IMapper mapper, IDatabaseContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<PaginatedList<UserBriefDto>> Handle(GetUsersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .ProjectToPaginatedListAsync<UserBriefDto>(_mapper.ConfigurationProvider, request.PageNumber, request.PageSize);
    }
}


