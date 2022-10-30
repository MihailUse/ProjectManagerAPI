using Microsoft.EntityFrameworkCore;

namespace DAL.Entities
{
    [Index(nameof(Login), IsUnique = true)]
    public class User : ITimestamp, ISoftDeletable
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string? About { get; set; }
        public byte[] Avatar { get; set; }
        public string PasswordHash { get; set; }

        public List<Task> Tasks { get; set; }
        public List<Project> Projects { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Assignee> Assignees { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
