using FluentValidation;

namespace Application.DTO.User;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Login)
            .MinimumLength(4)
            .MaximumLength(64)
            .WithMessage("Login at least greater than 2 and less than 16");

        RuleFor(x => x.About)
            .MaximumLength(512);
    }
}