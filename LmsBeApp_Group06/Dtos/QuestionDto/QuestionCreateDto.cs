using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class QuestionCreateDto
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public ICollection<AnswerCreateDto> Answers { get; set; }
    }
}
