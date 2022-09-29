using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<QuestionCreateDto, Question>();
            CreateMap<QuestionChangeDto, Question>();
            CreateMap<Question, QuestionReadDto>();
            CreateMap<Question, QuestionReadIdDto>();
            CreateMap<Question, QuestionSubmissionQuizRead>();
        }
    }
}
