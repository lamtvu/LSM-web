using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class UserChangePasswordDto
    {
        [Required]
        public string Oldpass { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Newpass { get; set; }
        [Required]
        public string Confirmpass { get; set; }
    }
}
