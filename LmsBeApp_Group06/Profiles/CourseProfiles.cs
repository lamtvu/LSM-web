using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseCreateDto, Course>();
            CreateMap<Course, CourseReadDto>();
            CreateMap<CourseChangeDto, Course>();
        }
    }
}