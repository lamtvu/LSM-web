using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Profiles
{
    public class ContentProfile : Profile
    {
        public ContentProfile()
        {
            CreateMap<ContentCreateDto,Content>();
            CreateMap<Content, ContentReadDto>();
        }
    }
}
