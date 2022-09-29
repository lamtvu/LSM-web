using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LmsBeApp_Group06.Models
{
    public class Exercise
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }

        public int ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public Class _Class { get; set; }

        public ICollection<SubmissionExercise> SubmissionExercises {get;set;}
    }
}
