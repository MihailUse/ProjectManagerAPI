namespace API.Models.Auth
{
    public class TokenRequestModel
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}