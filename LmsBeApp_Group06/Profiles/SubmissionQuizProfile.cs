using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Profiles
{
    public class SubmissionQuizProfile : Profile
    {
        public SubmissionQuizProfile()
        {
            CreateMap<SubmissionQuiz,SubmissionQuizReadDto>();
        }
    }
}
