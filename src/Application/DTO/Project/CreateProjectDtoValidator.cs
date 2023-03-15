using FluentValidation;

namespace Application.DTO.Project;

public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .MaximumLength(32);

        RuleFor(x => x.Description)
            .MaximumLength(512);
    }
}