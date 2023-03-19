using FluentValidation;

namespace Application.DTO.Task;

public class SetAssigneesDtoValidator : AbstractValidator<SetAssigneesDto>
{
    public SetAssigneesDtoValidator()
    {
        RuleFor(x => x.MemberShipIds)
            .NotEmpty()
            .Must(x => x.Count < 6);
        RuleFor(x => x.MemberShipIds)
            .Must(x => x?.Distinct().Count() == x?.Count)
            .WithMessage("Cannot contain duplicates");
    }
}