using System;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Dtos
{
    public class ExerciseReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DueDate { get; set; }
        public ClassReadDto _Class { get; set; }
        public int ClassId { get; set; }
    }
}