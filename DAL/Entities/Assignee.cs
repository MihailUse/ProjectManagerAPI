using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    //[Keyless]
    public class Assignee
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }

        [Required] public User User { get; set; }
        [Required] public Task Task { get; set; }
    }
}