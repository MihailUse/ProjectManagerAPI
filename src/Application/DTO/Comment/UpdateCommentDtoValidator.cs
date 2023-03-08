using FluentValidation;

namespace Application.DTO.Comment;

public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
{
    public UpdateCommentDtoValidator()
    {
        RuleFor(x => x.Text).MinimumLength(3);
    }
}