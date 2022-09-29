using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class ClassChangeDtos
    {
        [MaxLength(50)]
        [MinLength(3)]
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
