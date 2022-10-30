namespace API.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string About { get; set; }
        public byte[] Avatar { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
