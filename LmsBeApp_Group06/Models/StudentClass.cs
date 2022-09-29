using System.ComponentModel.DataAnnotations.Schema;

namespace LmsBeApp_Group06.Models
{
    public class StudentClass
    {
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public User Student { get; set; }

        public int ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public Class _Class { get; set; }
    }
}
