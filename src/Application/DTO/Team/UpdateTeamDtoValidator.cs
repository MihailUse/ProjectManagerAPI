using FluentValidation;

namespace Application.DTO.Team;

public class UpdateTeamDtoValidator : AbstractValidator<UpdateTeamDto>
{
    public UpdateTeamDtoValidator()
    {
        Include(new CreateTeamDtoValidator());
    }
}