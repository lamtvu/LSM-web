using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LmsBeApp_Group06.Attributes;

namespace LmsBeApp_Group06.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public bool IsLock { get; set; }

        [Required]
        [Gender]
        public string Gender { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string FullName { get; set; }
        public string Image { get; set; }

        [MaxLength(15)]
        [Phone]
        public string Phone { get; set; }

        public int RoleId { get; set; }

        [Required]
        public bool Verify { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public Information Information { get; set; }
        public RequestTeacher requestTeacher { get; set; }
        public ICollection<Class> StudingClasses { get; set; }
        public ICollection<Class> OwnedClasses { get; set; }
        public ICollection<Class> ClassAdminOfClasses { get; set; }
        public ICollection<Class> AssistantOfClasses { get; set; }
        public ICollection<SubmissionExercise> SubmissionExercises { get; set; }
        public ICollection<SubmissionQuiz> SubmissionQuizzes { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
        public ICollection<RequestStudent> RequestStudents { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Progress> Progresses { get; set; }
    }
}