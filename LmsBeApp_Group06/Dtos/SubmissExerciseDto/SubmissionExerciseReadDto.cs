using System;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Dtos
{
    public class SubmissionExerciseReadDto
    {
        public int Id { get; set; }
        public int? Core { get; set; }
        public string Comment { get; set; }
        public string FileType { get; set; }
        public DateTime SubmitDate { get; set; }
        public int ExerciseId { get; set; }
        public ExerciseReadDto Exercise { get; set; }
        public int StudentId { get; set; }
        public UserReadDto Student { get; set; }
    }
}