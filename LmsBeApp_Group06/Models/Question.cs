using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LmsBeApp_Group06.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }

        public int QuizId { get; set; }

        [ForeignKey(nameof(QuizId))]
        public Quiz Quiz { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
