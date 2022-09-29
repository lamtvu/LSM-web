using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<Content> Contents { get; set; }
    }
}
