using Application.DTO.Common;
using FluentValidation;

namespace Application.DTO.Status;

public class GetStatusesDtoValidator : AbstractValidator<SearchStatusDto>
{
    public GetStatusesDtoValidator()
    {
        Include(new PaginatedListQueryValidator());
    }
}