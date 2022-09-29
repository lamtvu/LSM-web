using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Models
{
    public class Report
    {
        [Required]
        [Key]
        public int Id { get; set; }       

        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(255)]
        public string Content { get; set; }

        public int SenderId { get; set; }

        [Required]
        [ForeignKey(nameof(SenderId))]
        public User Sender { get; set; }

        public int ClassId { get; set; }

        [Required]
        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; }
    }
}
