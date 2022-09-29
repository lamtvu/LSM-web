using System;
using System.ComponentModel.DataAnnotations;
using LmsBeApp_Group06.Attributes;

namespace LmsBeApp_Group06.Dtos
{
    public class QuizCreateDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        [Time]
        public string StartTime { get; set; }

        [Required]
        public int Duration { get; set; }
    }
}
