using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data
{
    public class LmsAppContext : DbContext
    {
        public LmsAppContext(DbContextOptions<LmsAppContext> opt) : base(opt) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ClassCourse> ClassCourses { get; set; }
        public DbSet<Section> sections { get; set; }
        public DbSet<Information> Informations { get; set; }
        public DbSet<RequestTeacher> RequestTeachers { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<SubmissionExercise> SubmissionExercises { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<SubmissionQuiz> SubmisstionQuizzes { get; set; }
        public DbSet<QuizAnswer> QuizAnswers { get; set; }
        public DbSet<RequestStudent> RequestStudents { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Progress> Progresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //user_teacher
            modelBuilder.Entity<User>()
            .HasMany(u => u.OwnedClasses)
            .WithOne(c => c.Teacher)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
            .HasMany(u => u.StudingClasses)
            .WithMany(c => c.students)
            .UsingEntity<StudentClass>(
                j => j.HasOne(sc => sc._Class).WithMany().OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne(sc => sc.Student).WithMany().OnDelete(DeleteBehavior.Restrict)
            );

            //class
            modelBuilder.Entity<Class>()
            .HasOne(c => c.Assistant)
            .WithMany(u => u.AssistantOfClasses)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Class>()
            .HasOne(c => c.ClassAdmin)
            .WithMany(u => u.ClassAdminOfClasses)
            .OnDelete(DeleteBehavior.Restrict);

            //coures
            modelBuilder.Entity<Course>()
            .HasMany(c => c.ClassesInUse)
            .WithMany(c => c.Courses)
            .UsingEntity<ClassCourse>(
                j => j.HasOne(x => x._Class).WithMany().OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne(x => x._Course).WithMany().OnDelete(DeleteBehavior.Restrict)
            );

            // submissionExercise
            modelBuilder.Entity<SubmissionExercise>()
            .HasOne(x => x.Exercise)
            .WithMany(x => x.SubmissionExercises)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubmissionExercise>()
            .HasOne(x => x.Student)
            .WithMany(x => x.SubmissionExercises)
            .OnDelete(DeleteBehavior.Restrict);

            //SubmissQuiz
            modelBuilder.Entity<SubmissionQuiz>()
            .HasMany(x=>x.Answers)
            .WithMany(x=>x.SubmissionQuizzes)
            .UsingEntity<QuizAnswer>(
                x=>x.HasOne(x=>x.Answer).WithMany().OnDelete(DeleteBehavior.Restrict),
                x=>x.HasOne(x=>x.SubmissionQuiz).WithMany().OnDelete(DeleteBehavior.Restrict)
            );

            modelBuilder.Entity<SubmissionQuiz>()
            .HasOne(x=>x.Student)
            .WithMany(x=>x.SubmissionQuizzes)
            .OnDelete(DeleteBehavior.Restrict);

            //request student
            modelBuilder.Entity<RequestStudent>()
           .HasOne(c => c.Sender)
           .WithMany(u => u.RequestStudents)
           .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<RequestStudent>()
            .HasOne(c => c.Class)
            .WithMany(u => u.RequestStudents)
            .OnDelete(DeleteBehavior.Restrict);

            //invitation
            modelBuilder.Entity<Invitation>()
            .HasOne(c => c.Class)
            .WithMany(u => u.Invitations)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invitation>()
            .HasOne(c => c.Receiver)
            .WithMany(u => u.Invitations)
            .OnDelete(DeleteBehavior.Restrict);

            //report
            modelBuilder.Entity<Report>()
            .HasOne(c => c.Class)
            .WithMany(u => u.ReportOfStudents)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
            .HasOne(c => c.Sender)
            .WithMany(u => u.Reports)
            .OnDelete(DeleteBehavior.Restrict);

            //review
            modelBuilder.Entity<Review>()
            .HasOne(c => c.Course)
            .WithMany(u => u.Reviews)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
            .HasOne(c => c.Sender)
            .WithMany(u => u.Reviews)
            .OnDelete(DeleteBehavior.Restrict);

            //progress
            modelBuilder.Entity<Progress>()
            .HasOne(c => c.Content)
            .WithMany(u => u.Progresses)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Progress>()
            .HasOne(c => c.User)
            .WithMany(u => u.Progresses)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}