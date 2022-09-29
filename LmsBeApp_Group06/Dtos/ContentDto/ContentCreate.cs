using System.ComponentModel.DataAnnotations;
using System.IO;

namespace LmsBeApp_Group06.Dtos
{
    public class ContentCreateDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Name { get; set; }
    }

    public class ContentReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
