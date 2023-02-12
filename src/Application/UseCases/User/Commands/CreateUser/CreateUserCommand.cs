using Application.Common.DTO.Auth;
using MediatR;

namespace Application.UseCases.Commands.CreateUser;

public class CreateUserCommand : IRequest<AccessTokensDto>
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}
