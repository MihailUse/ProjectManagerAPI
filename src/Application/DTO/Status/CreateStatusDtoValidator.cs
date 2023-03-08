using FluentValidation;

namespace Application.DTO.Status;

public class CreateStatusDtoValidator : AbstractValidator<CreateStatusDto>
{
    public CreateStatusDtoValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .MaximumLength(16);
    }
}