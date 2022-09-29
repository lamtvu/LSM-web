namespace LmsBeApp_Group06.Dtos
{
    public class CourseReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Level { get; set; }
        public bool IsPublic { get; set; }
        public string Description { get; set; }
        public int InstructorId { get; set; }
        public UserReadDto Instructor { get; set; }
        public bool IsLook { get; set; }
    }
}
