using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Profiles
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile()
        {
            CreateMap<AnswerCreateDto,Answer>();
            CreateMap<Answer,AnswerReadDto>();
            CreateMap<Answer,AnswerSubmissionQuizRead>();
        }
    }
}
