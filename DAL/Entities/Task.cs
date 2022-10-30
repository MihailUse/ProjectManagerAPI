using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Task : ITimestamp, ISoftDeletable
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [Required] public User Owner { get; set; }      
        [Required] public Status Status { get; set; }
        [Required] public Project Project { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Assignee> Assignees { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
