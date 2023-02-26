using FluentValidation;

namespace Application.DTO.Common;

public class PaginatedListQueryValidator : AbstractValidator<PaginatedListQuery>
{
    public PaginatedListQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber at least greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50)
            .WithMessage("PageSize at least greater than 0 and less than 51");
    }
}