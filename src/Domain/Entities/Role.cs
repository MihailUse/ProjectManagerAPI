namespace Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public List<MemberShip> MemberShips { get; set; } = null!;

    public Role(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }
}