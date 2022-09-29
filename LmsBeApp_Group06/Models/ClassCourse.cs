using System.ComponentModel.DataAnnotations.Schema;

namespace LmsBeApp_Group06.Models
{
    public class ClassCourse
    {
        public int ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public Class _Class { get; set; }

        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course _Course { get; set; }
    }
}
