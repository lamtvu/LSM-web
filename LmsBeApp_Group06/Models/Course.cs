using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LmsBeApp_Group06.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [Required]
        public bool IsLook { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Name { get; set; }
        public string Image { get; set; }

        [Range(0, 2)]
        [Required]
        public int Level { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        public string Description { get; set; }

        public int InstructorId { get; set; }
        [ForeignKey(nameof(InstructorId))]
        public User Instructor { get; set; }

        public ICollection<Class> ClassesInUse { get; set; }

        public ICollection<Section> Sections {get;set;}
        public ICollection<Review> Reviews { get; set; }
    }
}
