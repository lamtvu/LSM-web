using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LmsBeApp_Group06.Models
{
    public class SubmissionExercise
    {
        [Key]
        public int Id { get; set; }

        [Range(0, 10)]
        public int? Core { get; set; }

        [MaxLength(100)]
        [MinLength(50)]
        public string Comment { get; set; }
        public string FileType { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SubmitDate { get; set; }

        public int ExerciseId { get; set; }
        [ForeignKey(nameof(ExerciseId))]
        public Exercise Exercise { get; set; }

        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public User Student { get; set; }
    }
}
