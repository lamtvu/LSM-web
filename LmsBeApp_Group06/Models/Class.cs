using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LmsBeApp_Group06.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }
        public int TeacherId { get; set; }

        [ForeignKey(nameof(TeacherId))]
        [Required]
        public User Teacher { get; set; }

        public int? ClassAdminId { get; set; }

        [ForeignKey(nameof(ClassAdminId))]
        public User ClassAdmin { get; set; }

        public int? AssistantId { get; set; }

        [ForeignKey(nameof(AssistantId))]
        public User Assistant { get; set; }

        [MaxLength(50)]
        [MinLength(3)]
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime CreateDate { get; set; }
        public ICollection<User> students { get; set; }

        public ICollection<Course> Courses { get; set; }

        public ICollection<Exercise> Exercises { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
        public ICollection<RequestStudent> RequestStudents { get; set; }
        public ICollection<Report> ReportOfStudents { get; set; }
        public ICollection<Announcement> Announcements { get; set; }

    }

}
