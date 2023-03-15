using Application.DTO.Common;
using Domain.Enums;

namespace Application.DTO.MemberShip;

public record SearchMemberShipDto : PaginatedListQuery
{
    public Role? Role { get; set; }
    public string? SearchByUserName { get; set; }
}