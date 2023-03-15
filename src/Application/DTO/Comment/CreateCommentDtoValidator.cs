using FluentValidation;

namespace Application.DTO.Comment;

public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
{
    public CreateCommentDtoValidator()
    {
        RuleFor(x => x.Text).MinimumLength(3);
    }
}