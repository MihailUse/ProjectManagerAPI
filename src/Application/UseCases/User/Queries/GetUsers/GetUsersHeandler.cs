using Application.Common.DTO;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Mappings;
using AutoMapper;
using MediatR;

namespace Application.UseCases.User.Queries.GetUsersWithPagination;

public class GetUsersHeandler : IRequestHandler<GetUsersQuery, PaginatedList<UserBriefDto>>
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _context;

    public GetUsersHeandler(IMapper mapper, IDatabaseContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<PaginatedList<UserBriefDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .ProjectToPaginatedListAsync<UserBriefDto>(_mapper.ConfigurationProvider, request.PageNumber, request.PageSize);
    }
}
