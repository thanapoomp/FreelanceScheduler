using System.ComponentModel.DataAnnotations;

namespace freelance.api.Dtos
{
    public class UserForLoginDto
    {
        [Required]
        [MinLength(4)]
        public string Username { get; set; }
        [Required]
        [MinLength(4)]
        public string Password { get; set; }
    }
}