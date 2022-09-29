using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LmsBeApp_Group06.Models
{
    public class RequestTeacher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public int UserId { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public User User {get;set;}
    }
}
