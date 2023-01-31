﻿using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Project : ITimestamp, ISoftDeletable
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public byte[] Logo { get; set; } = null!;
        public string? Description { get; set; }

        public List<Status> Statuses { get; set; } = null!;
        public List<Task> Tasks { get; set; } = null!;
        public List<MemberShip> Memberships { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
