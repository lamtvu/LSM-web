using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Models
{
    public class Review
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int Star { get; set; }

        [MaxLength(255)]
        public string Comment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public int CourseId { get; set; }

        [Required]
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }

        public int SenderId { get; set; }

        [Required]
        [ForeignKey(nameof(SenderId))]
        public User Sender { get; set; }
    }
}
