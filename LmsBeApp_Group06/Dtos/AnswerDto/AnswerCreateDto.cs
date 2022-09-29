using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class AnswerCreateDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
    }
}
