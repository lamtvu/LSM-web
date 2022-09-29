using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Models
{
    public class Information
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public int UserId { get; set; }

        public User User {get;set;}
    }
}
