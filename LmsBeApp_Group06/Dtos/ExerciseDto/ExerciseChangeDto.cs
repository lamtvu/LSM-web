using System;
using System.ComponentModel.DataAnnotations;

namespace LmsBeApp_Group06.Dtos
{
    public class ExerciseChangeDto
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
        [DataType(DataType.Time)]
        public string DueTime{get;set;}
    }
}
