using FluentValidation;

namespace Application.DTO.Task;

public class SetAssigneesDtoValidator : AbstractValidator<SetAssigneesDto>
{
    public SetAssigneesDtoValidator()
    {
        RuleFor(x => new { x.AssigneeIds, x.AssigneeTeamId })
            .Must(x => x.AssigneeIds is { Count: > 0 } || x.AssigneeTeamId != default);
        RuleFor(x => x.AssigneeIds)
            .Must(x => x?.Distinct().Count() == x?.Count)
            .WithMessage("Cannot contain duplicates");
    }
}