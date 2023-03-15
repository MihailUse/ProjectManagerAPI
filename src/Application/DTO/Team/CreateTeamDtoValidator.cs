using FluentValidation;

namespace Application.DTO.Team;

public class CreateTeamDtoValidator : AbstractValidator<CreateTeamDto>
{
    public CreateTeamDtoValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .MaximumLength(16);
        RuleFor(x => x.Description)
            .MaximumLength(512);
        RuleFor(x => x.Color).Length(7);
    }
}