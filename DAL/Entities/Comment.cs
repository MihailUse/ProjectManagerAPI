using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Comment : ITimestamp, ISoftDeletable
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        [Required] public User Owner { get; set; }
        [Required] public Task Task { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}