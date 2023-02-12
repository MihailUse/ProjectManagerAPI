using Application.Common.Interfaces;
using MediatR;

namespace Application.Common.Behaviours;

internal class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IDatabaseContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AuthorizationBehaviour(IDatabaseContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_currentUserService.GetUserId() == default)
            throw new UnauthorizedAccessException();

        return await next();
    }
}
