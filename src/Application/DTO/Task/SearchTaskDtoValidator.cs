using Application.DTO.Common;
using FluentValidation;

namespace Application.DTO.Task;

public class GetTasksDtoValidator : AbstractValidator<SearchTaskDto>
{
    public GetTasksDtoValidator()
    {
        Include(new PaginatedListQueryValidator());
    }
}