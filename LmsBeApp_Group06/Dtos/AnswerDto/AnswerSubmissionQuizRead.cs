namespace LmsBeApp_Group06.Dtos
{
    public class AnswerSubmissionQuizRead
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public QuestionSubmissionQuizRead Question { get; set; }
}
}