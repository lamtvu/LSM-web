using System.Collections.Generic;

namespace LmsBeApp_Group06.Dtos
{
    public class SectionReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CourseId { get; set; }
        public ICollection<ContentReadDto> Contents { get; set; }
    }
}