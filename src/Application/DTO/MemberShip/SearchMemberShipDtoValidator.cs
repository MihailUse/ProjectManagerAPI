using Application.DTO.Common;
using FluentValidation;

namespace Application.DTO.MemberShip;

public class SearchMemberShipDtoValidator : AbstractValidator<SearchMemberShipDto>
{
    public SearchMemberShipDtoValidator()
    {
        Include(new PaginatedListQueryValidator());
        RuleFor(x => x.ProjectId).Must(x => x != default);
        RuleFor(x => x.Role).IsInEnum();
    }
}