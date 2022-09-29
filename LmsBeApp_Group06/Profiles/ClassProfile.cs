using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Profiles
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<Class, ClassReadDto>();
            CreateMap<ClassCreateDtos, Class>();
            CreateMap<ClassChangeDtos, Class>();
        }
    }
}
