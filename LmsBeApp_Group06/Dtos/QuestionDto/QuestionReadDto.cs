using System.Collections.Generic;

namespace LmsBeApp_Group06.Dtos
{
    public class QuestionReadDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int QuizId { get; set; }
        public ICollection<AnswerReadDto> Answers { get; set; }
    }
}
