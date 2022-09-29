using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class AnswerReadDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }
}
