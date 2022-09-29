using System;
using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Profiles
{
    public class QuizProfile : Profile
    {
        public QuizProfile()
        {
            CreateMap<QuizCreateDto, Quiz>();
            CreateMap<Quiz, QuizReadDto>()
            .ForMember(x => x.CreateDate, opt => opt.MapFrom<CreateDateResolve>())
            .ForMember(x => x.StartDate, opt => opt.MapFrom<StartDateResolve>());
        }

        public class CreateDateResolve : IValueResolver<Quiz, QuizReadDto, DateTime>
        {
            public DateTime Resolve(Quiz source, QuizReadDto destination, DateTime destMember, ResolutionContext context)
            {
                return source.CreateDate.AddHours(7);
            }
        }
        public class StartDateResolve : IValueResolver<Quiz, QuizReadDto, DateTime>
        {
            public DateTime Resolve(Quiz source, QuizReadDto destination, DateTime destMember, ResolutionContext context)
            {
                return source.StartDate.AddHours(7);
            }
        }
    }
}
