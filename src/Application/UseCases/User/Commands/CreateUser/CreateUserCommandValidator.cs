using FluentValidation;

namespace Application.UseCases.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Login)
            .MinimumLength(4)
            .MaximumLength(64)
            .WithMessage("Login at least greater than 2 and less than 16");

        RuleFor(x => x.Password)
            .MinimumLength(4)
            .MaximumLength(64)
            .WithMessage("Password at least greater than 4 and less than 64");
    }
}
