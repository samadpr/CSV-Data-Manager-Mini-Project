using System.ComponentModel.DataAnnotations;

namespace CsvManagerAPI.API.User.RequestObject
{
    public class LoginRequestObject
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
