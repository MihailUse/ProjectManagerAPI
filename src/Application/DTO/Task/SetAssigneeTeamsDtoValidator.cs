using FluentValidation;

namespace Application.DTO.Task;

public class SetAssigneeTeamsDtoValidator : AbstractValidator<SetAssigneeTeamsDto>
{
    public SetAssigneeTeamsDtoValidator()
    {
        RuleFor(x => x.TeamIds)
            .NotEmpty()
            .Must(x => x.Count < 6);
        RuleFor(x => x.TeamIds)
            .Must(x => x?.Distinct().Count() == x?.Count)
            .WithMessage("Cannot contain duplicates");
    }
}