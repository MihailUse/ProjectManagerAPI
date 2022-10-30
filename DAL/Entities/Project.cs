using Microsoft.EntityFrameworkCore;

namespace DAL.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Project : ITimestamp, ISoftDeletable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public string Description { get; set; }

        public List<Task> Tasks { get; set; }
        public List<Status> Statuses { get; set; }
        public List<MemberShip> Memberships { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
