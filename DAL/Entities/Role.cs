using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public List<MemberShip> MemberShips { get; set; } = null!;
}