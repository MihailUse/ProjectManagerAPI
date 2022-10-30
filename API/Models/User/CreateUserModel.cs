using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.User
{
    public class CreateUserModel
    {
        [MinLength(3), MaxLength(32)]
        public string Login { get; set; }

        [MaxLength(256)]
        public string? About { get; set; }

        [MinLength(6), MaxLength(128)]
        public string Password { get; set; }

        [MinLength(6), MaxLength(128), Compare(nameof(Password))]
        public string RetryPassword { get; set; }

        [FromForm, NotMapped]
        public IFormFile? Avatar { get; set; }
    }
}
