using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Attributes;

namespace LmsBeApp_Group06.Models
{
    public class Announcement
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(255)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [Required]
        [Attributes.Type]
        public string Type { get; set; }

        public int ClassId { get; set; }

        [Required]
        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; }
    }
}
