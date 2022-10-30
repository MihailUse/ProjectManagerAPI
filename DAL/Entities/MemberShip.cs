using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class MemberShip
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }

        [Required] public User User { get; set; }
        [Required] public Project Project { get; set; }
        [Required] public Role Role { get; set; }
    }
}
