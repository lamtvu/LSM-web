using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class SubmissionExerciseChangeDto
    {
        [Required]
        [Range(0, 10)]
        public int? Core { get; set; }
        public string Comment { get; set; }
    }
}