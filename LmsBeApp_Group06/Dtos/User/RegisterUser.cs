using LmsBeApp_Group06.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Dtos
{
    public class RegisterUser
    {
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Password { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Gender]
        [Required]
        public string Gender { get; set; }
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }
        [Phone]
        public string Phone { get; set; }
    }
}
