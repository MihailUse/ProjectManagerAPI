using FluentValidation;

namespace Application.DTO.Project;

public class UpdateProjectDtoValidator : AbstractValidator<UpdateProjectDto>
{
    public UpdateProjectDtoValidator()
    {
        Include(new CreateProjectDtoValidator());
    }
}