using Application.Common.DTO.Auth;
using Application.Common.Interfaces;
using MediatR;

namespace Application.UseCases.Commands.CreateUser;

public class CreateUserCommandHeandler : IRequestHandler<CreateUserCommand, AccessTokensDto>
{
    private readonly IIdentityService _identityService;

    public CreateUserCommandHeandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AccessTokensDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.CreateUserAsync(request.Login, request.Password);
    }
}
