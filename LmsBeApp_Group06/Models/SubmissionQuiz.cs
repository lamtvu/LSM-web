using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LmsBeApp_Group06.Models
{
    public class SubmissionQuiz
    {
        [Key]
        public int Id { get; set; }
        public float Core { get; set; }
        public string Comment { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public bool IsFinish { get; set; }
        public int QuizId { get; set; }
        [ForeignKey(nameof(QuizId))]
        public Quiz Quiz { get; set; }
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public User Student { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
