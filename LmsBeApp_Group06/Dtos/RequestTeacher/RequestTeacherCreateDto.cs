using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class RequestTeacherCreateDto
    {
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public string CompanyName { get; set; }
    }
}