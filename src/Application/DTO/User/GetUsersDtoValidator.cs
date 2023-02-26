using Application.DTO.Common;
using FluentValidation;

namespace Application.DTO.User;

public class GetUsersDtoValidator : AbstractValidator<GetUsersDto>
{
    public GetUsersDtoValidator()
    {
        Include(new PaginatedListQueryValidator());
    }
}