using FluentValidation;

namespace Application.UseCases.User.Queries.GetUsersWithPagination;

public class GetUsersWithPaginationQueryValidator : AbstractValidator<GetUsersWithPaginationQuery>
{
    public GetUsersWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50)
            .WithMessage("PageNumber at least greater than 0 and less than 51");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50)
            .WithMessage("PageSize at least greater than 0 and less than 51");
    }
}
