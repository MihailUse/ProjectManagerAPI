using FluentValidation;

namespace Application.DTO.Task;

public class SetAssigneesDtoValidator : AbstractValidator<SetAssigneesDto>
{
    public SetAssigneesDtoValidator()
    {
        RuleFor(x => x.MemberShipIds)
            .NotNull()
            .Must(x => x.Count < 6)
            .WithMessage("Max assignees is 5");
        RuleFor(x => x.MemberShipIds)
            .Must(x => x?.Distinct().Count() == x?.Count)
            .WithMessage("Cannot contain duplicates");
    }
}