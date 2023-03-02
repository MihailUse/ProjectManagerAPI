using FluentValidation;

namespace Application.DTO.MemberShip;

public class CreateMemberShipDtoValidator : AbstractValidator<CreateMemberShipDto>
{
    public CreateMemberShipDtoValidator()
    {
        RuleFor(x => x.Role).IsInEnum();
    }
}