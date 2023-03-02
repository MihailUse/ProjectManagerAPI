using FluentValidation;

namespace Application.DTO.MemberShip;

public class UpdateMemberShipDtoValidator : AbstractValidator<UpdateMemberShipDto>
{
    public UpdateMemberShipDtoValidator()
    {
        RuleFor(x => x.Role).IsInEnum();
    }
}