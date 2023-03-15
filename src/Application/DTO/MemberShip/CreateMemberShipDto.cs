using Domain.Enums;

namespace Application.DTO.MemberShip;

public class CreateMemberShipDto
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public Role Role { get; set; } = Role.MemberShip;
}