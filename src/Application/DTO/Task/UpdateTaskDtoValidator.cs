using FluentValidation;

namespace Application.DTO.Task;

public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .MinimumLength(3)
            .MaximumLength(32);
    }
}