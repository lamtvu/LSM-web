
using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}