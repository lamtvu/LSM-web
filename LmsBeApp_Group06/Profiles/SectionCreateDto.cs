using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class SectionCreateDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
