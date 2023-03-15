using Application.DTO.Common;
using FluentValidation;

namespace Application.DTO.Team;

public class SearchTeamDtoValidator : AbstractValidator<SearchTeamDto>
{
    public SearchTeamDtoValidator()
    {
        Include(new PaginatedListQueryValidator());
    }
}