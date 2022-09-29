using System.ComponentModel.DataAnnotations.Schema;

namespace LmsBeApp_Group06.Models
{
    public class QuizAnswer
    {
        public int SubmissionQuizId { get; set; }
        [ForeignKey(nameof(SubmissionQuizId))]
        public SubmissionQuiz SubmissionQuiz { get; set; }

        public int AnswerId { get; set; }
        [ForeignKey(nameof(AnswerId))]
        public Answer Answer { get; set; }
    }
}
