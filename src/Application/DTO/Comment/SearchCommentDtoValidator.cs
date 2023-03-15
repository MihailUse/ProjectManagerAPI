using Application.DTO.Common;
using FluentValidation;

namespace Application.DTO.Comment;

public class SearchCommentDtoValidator : AbstractValidator<SearchCommentDto>
{
    public SearchCommentDtoValidator()
    {
        Include(new PaginatedListQueryValidator());
    }
}