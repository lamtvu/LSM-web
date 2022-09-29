using System;

namespace LmsBeApp_Group06.Dtos
{
    public class QuizReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public int ClassId { get; set; }
    }
}
