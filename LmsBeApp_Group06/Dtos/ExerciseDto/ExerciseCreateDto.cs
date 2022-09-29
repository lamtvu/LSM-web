using System;
using System.ComponentModel.DataAnnotations;
using LmsBeApp_Group06.Attributes;

namespace LmsBeApp_Group06.Dtos
{
    public class ExerciseCreateDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [Time]
        public string DueTime { get; set; }
    }
}
