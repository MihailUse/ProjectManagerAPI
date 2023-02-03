﻿namespace Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public List<MemberShip> MemberShips { get; set; } = null!;
}