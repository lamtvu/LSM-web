using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Dtos.RequestStudent
{
    public class ClassRequestReadDto
    {
        public ClassReadDto ClassReadDto { get; set; }
        public bool IsRequest { get; set; }
    }
}