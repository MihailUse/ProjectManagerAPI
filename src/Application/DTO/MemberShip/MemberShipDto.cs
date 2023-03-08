using Application.DTO.User;
using Domain.Enums;

namespace Application.DTO.MemberShip;

public class MemberShipDto
{
    public Guid Id { get; set; }
    public UserBriefDto User { get; set; } = null!;
    public Role Role { get; set; }
}