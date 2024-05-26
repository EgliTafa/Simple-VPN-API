using System.ComponentModel.DataAnnotations;

namespace Simple_VPN_API.Dto
{
    public class UserRegistrationDto
    {
        [Required]
        [MaxLength(20)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(60)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(60)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
