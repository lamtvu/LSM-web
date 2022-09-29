using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class CourseChangeDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Name { get; set; }
        public byte[] Image { get; set; }

        [Range(0, 2)]
        [Required]
        public int Level { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        public string Description { get; set; }
    }
}
