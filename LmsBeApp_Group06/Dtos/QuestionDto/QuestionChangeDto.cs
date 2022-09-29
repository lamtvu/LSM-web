using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class QuestionChangeDto
    {
        [Required]
        public string Content { get; set; }
        public ICollection<AnswerCreateDto> Answers { get; set; }
    }
}
