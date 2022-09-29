using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Dtos
{
    public class SubmissionQuizReadDto
    {
        public int Id { get; set; }
        public float Core { get; set; }
        public string Comment { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public bool IsFinish { get; set; }
        public int QuizId { get; set; }
        public QuizReadDto Quiz { get; set; }
        public int StudentId { get; set; }
        public UserReadDto Student { get; set; }
        public ICollection<AnswerSubmissionQuizRead> Answers { get; set; }
    }
}
